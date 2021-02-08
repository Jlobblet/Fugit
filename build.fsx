#r "paket:
nuget Fantomas.Extras
nuget Fake.DotNet.Cli
nuget Fake.DotNet.NuGet
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"

open System
open Fantomas
open Fantomas.Extras
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

[<Literal>]
let solution = "Fugit.sln"

[<Literal>]
let NugetSource = "https://api.nuget.org/v3/index.json"

let (|NullOrWhitespaceString|ValueString|) (str: string) =
    if String.isNullOrWhiteSpace str then NullOrWhitespaceString else ValueString str

let tryGetEnvironmentVariable name =
    let tryString =
        function
        | NullOrWhitespaceString -> None
        | ValueString var -> Some var

    [ Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process)
      Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User)
      Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine) ]
    |> List.choose tryString
    |> List.tryHead

let getEnvironmentVariable name =
    match tryGetEnvironmentVariable name with
    | None -> failwith $"Could not find environment variable %s{name}"
    | Some var -> var

// Targets

Target.initEnvironment ()

// Linting

Target.create "Check F# formatting"
<| fun _ ->
    let result =
        !! "**/*.fs"
        -- "**/bin/**/*.fs"
        -- "**/obj/**/*.fs"
        |> FakeHelpers.checkCode
        |> Async.RunSynchronously

    if result.IsValid then
        Trace.log "No files need formatting"
    elif result.NeedsFormatting then
        Trace.log "The following files need formatting:"
        result.Formatted |> List.iter Trace.log
        failwith "Some files need formatting, check output for more info"
    else
        Trace.log $"Errors while formatting: {result.Errors}"

Target.create "Format F# code"
<| fun _ ->
    let formattedFiles =
        !! "**/*.fs"
        -- "**/bin/**/*.fs"
        -- "**/obj/**/*.fs"
        |> FakeHelpers.formatCode
        |> Async.RunSynchronously

    if not << Array.isEmpty <| formattedFiles then
        Trace.log "Formatted files:"
        formattedFiles |> Array.iter (Trace.logfn "%s")
    else
        Trace.log "No files needed formatting"

// Cleaning

Target.create "Clean"
<| fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ "tests/**/bin"
    ++ "tests/**/obj"
    ++ "NuGet"
    |> Shell.cleanDirs

// Restore

Target.create "Restore"
<| fun _ -> solution |> DotNet.restore id

// Build

Target.create "Build"
<| fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (
        DotNet.build
            (fun o ->
                { o with
                      Configuration = DotNet.BuildConfiguration.Release
                      NoLogo = true
                      NoRestore = true })
    )

Target.create "Build tests"
<| fun _ ->
    !! "tests/**/*.*proj"
    |> Seq.iter (
        DotNet.build
            (fun o ->
                { o with
                      NoLogo = true
                      NoRestore = true })
    )

// Running tests

Target.create "Run tests"
<| fun _ ->
    !! "tests/**/*.*proj"
    |> Seq.iter (
        DotNet.test
            (fun o ->
                { o with
                      NoLogo = true
                      NoRestore = true })
    )

// NuGet

Target.create "Create NuGet packages"
<| fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (
        DotNet.pack
            (fun o ->
                { o with
                      NoLogo = true
                      OutputPath = Some "NuGet" })
    )

Target.create "Push NuGet packages"
<| fun _ ->
    let apiKey = getEnvironmentVariable "NUGET_KEY"

    !! "NuGet/Fugit.*.nupkg"
    |> Seq.iter (
        DotNet.nugetPush
            (fun o ->
                { o with
                      PushParams =
                          { o.PushParams with
                                ApiKey = Some apiKey
                                Source = Some NugetSource } })
    )

// Combinations

Target.create "All" ignore
Target.create "Deploy" ignore

// Dependencies

"Clean" ==> "Restore" ==> "Build"

"Build" ==> "Run tests"

"Clean"
?=> "Format F# code"
?=> "Check F# formatting"

"Check F# formatting" ?=> "Build" ?=> "Run tests"

"All" <== [ "Check F# formatting"; "Run tests" ]

"All"
==> "Create NuGet packages"
==> "Push NuGet packages"
==> "Deploy"

// Run

Target.runOrDefaultWithArguments "All"
