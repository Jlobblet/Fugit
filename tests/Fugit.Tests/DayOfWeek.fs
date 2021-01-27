module DayOfWeek

open System
open System.Collections.Generic
open Xunit
open Fugit
open Fugit.Shorthand
open Fugit.Months

let flip f b a = f a b

[<Fact>]
let ``check the first Tuesday before 28 January 2021 is 26 January 2021`` () =
    let before = DayOfWeek.beforeOrEqual

    let expected = January 26 2021

    let actual =
        DayOfWeek.theFirst DayOfWeek.Tuesday
        |> before (January 28 2021)

    Assert.Equal(expected, actual)

[<Fact>]
let ``check the first Tuesday after 26 January 2021 is 2 February 2021`` () =
    let after = DayOfWeek.after

    let expected = February 2 2021

    let actual =
        DayOfWeek.theFirst DayOfWeek.Tuesday
        |> after (January 26 2021)

    Assert.Equal(expected, actual)

[<Fact>]
let ``check that the first Tuesday after 28 January 2021 is 2 February 2021`` () =
    let after = DayOfWeek.after

    let expected = February 2 2021

    let actual =
        DayOfWeek.theFirst DayOfWeek.Tuesday
        |> after (January 28 2021)

    Assert.Equal(expected, actual)

[<Fact>]
let ``check that the first day of the week after or equal that day is the same date`` () =
    let afterOrEqual = DayOfWeek.afterOrEqual
    let theFirst = DayOfWeek.theFirst

    let input =
        [ 0 .. 1000 ]
        |> List.map (fun n -> n |> days |> after (January 28 2000))


    let expected = input

    let actual =
        input
        |> List.map
            (fun dt ->
                let day = dt.DayOfWeek
                theFirst day |> afterOrEqual dt)

    Assert.Equal<DateTime>(expected, actual)

[<Fact>]
let ``check that the first day the week after that day is one week later`` () =
    let input =
        [ 0 .. 1000 ]
        |> List.map (fun n -> n |> days |> after (January 28 2000))

    let expected =
        input |> List.map (oneWeek |> (flip after))

    let after = DayOfWeek.after
    let theFirst = DayOfWeek.theFirst


    let actual =
        input
        |> List.map
            (fun dt ->
                let day = dt.DayOfWeek
                theFirst day |> after dt)

    Assert.Equal<DateTime>(expected, actual)


// Some US holidays as tests

[<Fact>]
let ``work out the Birthday of Martin Luther King Jr. (the third Monday in January) in 2021`` () =
    let afterOrEqual = DayOfWeek.afterOrEqual
    let theThird = DayOfWeek.theThird

    let expected = January 18 2021

    let actual =
        theThird DayOfWeek.Monday
        |> afterOrEqual (January 1 2021)

    Assert.Equal(expected, actual)

/// 1 February 2021 is a Monday, so the 3rd Monday of February 2021 is the 15th
[<Fact>]
let ``work out the Birthday of George Washington (the third Monday in February) in 2021`` () =
    let afterOrEqual = DayOfWeek.afterOrEqual

    let expected = February 15 2021

    let actual =
        DayOfWeek.theThird DayOfWeek.Monday
        |> afterOrEqual (February 1 2021)

    Assert.Equal(expected, actual)

[<Fact>]
let ``work out the date of Labor Day (the first Monday in September) in 2021`` () =
    let afterOrEqual = DayOfWeek.afterOrEqual

    let expected = September 6 2021

    let actual =
        DayOfWeek.theFirst DayOfWeek.Monday
        |> afterOrEqual (September 1 2021)

    Assert.Equal(expected, actual)
