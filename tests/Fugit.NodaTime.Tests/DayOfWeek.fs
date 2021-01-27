module DayOfWeek

open NodaTime
open Xunit
open Fugit.NodaTime
open Fugit.NodaTime.Shorthand
open Fugit.NodaTime.Months

let flip f b a = f a b
let localDateToInstant (d: LocalDate) =
    d.AtMidnight().InUtc().ToInstant()

[<Fact>]
let ``check the first Tuesday before 28 January 2021 is 26 January 2021`` () =
    let before = DayOfWeek.beforeOrEqual

    let expected = January 26 2021 |> localDateToInstant

    let actual =
        DayOfWeek.theFirst IsoDayOfWeek.Tuesday
        |> before (January 28 2021 |> localDateToInstant)

    Assert.Equal(expected, actual)

[<Fact>]
let ``check the first Tuesday after 26 January 2021 is 2 February 2021`` () =
    let after = DayOfWeek.after

    let expected = February 2 2021 |> localDateToInstant

    let actual =
        DayOfWeek.theFirst IsoDayOfWeek.Tuesday
        |> after (January 26 2021 |> localDateToInstant)

    Assert.Equal(expected, actual)

[<Fact>]
let ``check that the first Tuesday after 28 January 2021 is 2 February 2021`` () =
    let after = DayOfWeek.after

    let expected = February 2 2021 |> localDateToInstant

    let actual =
        DayOfWeek.theFirst IsoDayOfWeek.Tuesday
        |> after (January 28 2021 |> localDateToInstant)

    Assert.Equal(expected, actual)

[<Fact>]
let ``check that the first day of the week after or equal that day is the same date`` () =
    let afterOrEqual = DayOfWeek.afterOrEqual
    let theFirst = DayOfWeek.theFirst

    let input =
        [ 0 .. 1000 ]
        |> List.map (fun n -> n |> days |> after (January 28 2000 |> localDateToInstant))


    let expected = input

    let actual =
        input
        |> List.map
            (fun dt ->
                let day = dt.InUtc().DayOfWeek
                theFirst day |> afterOrEqual dt)

    Assert.Equal<Instant>(expected, actual)

[<Fact>]
let ``check that the first day the week after that day is one week later`` () =
    let input =
        [ 0 .. 1000 ]
        |> List.map (fun n -> n |> days |> after (January 28 2000 |> localDateToInstant))

    let expected =
        input |> List.map (oneWeek |> (flip after))

    let after = DayOfWeek.after
    let theFirst = DayOfWeek.theFirst


    let actual =
        input
        |> List.map
            (fun dt ->
                let day = dt.InUtc().DayOfWeek
                theFirst day |> after dt)

    Assert.Equal<Instant>(expected, actual)


// Some US holidays as tests

[<Fact>]
let ``work out the Birthday of Martin Luther King Jr. (the third Monday in January) in 2021`` () =
    let afterOrEqual = DayOfWeek.afterOrEqual
    let theThird = DayOfWeek.theThird

    let expected = January 18 2021 |> localDateToInstant

    let actual =
        theThird IsoDayOfWeek.Monday
        |> afterOrEqual (January 1 2021 |> localDateToInstant)

    Assert.Equal(expected, actual)

/// 1 February 2021 is a Monday, so the 3rd Monday of February 2021 is the 15th
[<Fact>]
let ``work out the Birthday of George Washington (the third Monday in February) in 2021`` () =
    let afterOrEqual = DayOfWeek.afterOrEqual

    let expected = February 15 2021 |> localDateToInstant

    let actual =
        DayOfWeek.theThird IsoDayOfWeek.Monday
        |> afterOrEqual (February 1 2021 |> localDateToInstant)

    Assert.Equal(expected, actual)

[<Fact>]
let ``work out the date of Labor Day (the first Monday in September) in 2021`` () =
    let afterOrEqual = DayOfWeek.afterOrEqual

    let expected = September 6 2021 |> localDateToInstant

    let actual =
        DayOfWeek.theFirst IsoDayOfWeek.Monday
        |> afterOrEqual (September 1 2021 |> localDateToInstant)

    Assert.Equal(expected, actual)
