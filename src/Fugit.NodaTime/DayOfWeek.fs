module Fugit.NodaTime.DayOfWeek

open System
open NodaTime
open Fugit.NodaTime.Shorthand

type TheNthDay = { n: int; day: IsoDayOfWeek }

let tryTheNth N day =
    if N < 1
    then Error $"{nameof N} must be >= 1 (was {N})."
    else Ok { n = N; day = day }

let theNth N day =
    match tryTheNth N day with
    | Ok p -> p
    | _ ->
        raise
        <| ArgumentOutOfRangeException $"{nameof N} must be >= 1 (was {N})."

let theFirst = theNth 1
let theSecond = theNth 2
let theThird = theNth 3
let theFourth = theNth 4
let theFifth = theNth 5
let theSixth = theNth 6
let theSeventh = theNth 7
let theEight = theNth 8
let theNinth = theNth 9
let theTenth = theNth 10

type ZoneDependents(zone) =
    member this.zone: DateTimeZone = zone

    member this.beforeOrEqual (instant: Instant) nthDay =
        let initialDay =
            instant.InZone(this.zone).DayOfWeek |> unbox<int>

        let targetDay = nthDay.day |> unbox<int>

        let dayDifference = (initialDay - targetDay + 7) % 7
        let weekDifference = nthDay.n - 1

        (weekDifference |> weeks)
        + (dayDifference |> days)
        |> before instant

    member this.before (instant: Instant) nthDay =
        this.beforeOrEqual (oneDay |> before instant) nthDay

    member this.afterOrEqual (instant: Instant) nthDay =
        let initialDay =
            instant.InZone(this.zone).DayOfWeek |> unbox<int>

        let targetDay = nthDay.day |> unbox<int>

        let dayDifference = (targetDay - initialDay + 7) % 7
        let weekDifference = nthDay.n - 1

        (weekDifference |> weeks)
        + (dayDifference |> days)
        |> after instant

    member this.after (instant: Instant) nthDay =
        this.afterOrEqual (oneDay |> after instant) nthDay

let LocalZoneDependents =
    ZoneDependents(DateTimeZoneProviders.Bcl.GetSystemDefault())

let beforeOrEqual = LocalZoneDependents.beforeOrEqual
let before = LocalZoneDependents.before
let after = LocalZoneDependents.after
let afterOrEqual = LocalZoneDependents.afterOrEqual
