module Fugit.NodaTime.DayOfWeek

open System
open NodaTime
open Fugit.NodaTime.Shorthand

/// Use a function from Fugit.NodaTime.DayOfWeek to construct this record.
/// tryTheNth, theNth, theFirst (etc.)
///
/// Contains a day of the week and a number of weeks to add or subtract.
type TheNthDay = { n: int; day: IsoDayOfWeek }

/// Try to make a TheNthDay (record)
/// If N < 1, the function will return an Error
/// If N >= 1, the function will return an Ok TheNthDay
let tryTheNth N day =
    if N < 1
    then Error $"{nameof N} must be >= 1 (was {N})."
    else Ok { n = N; day = day }

/// Construct a TheNthDay (record)
/// If N < 1, the function will raise an ArgumentOutOfRangeException
/// If N >= 1, the function will return the result.
let theNth N day =
    match tryTheNth N day with
    | Ok p -> p
    | _ ->
        raise
        <| ArgumentOutOfRangeException $"{nameof N} must be >= 1 (was {N})."

/// Partially applied theNth with N = 1
let theFirst = theNth 1
/// Partially applied theNth with N = 2
let theSecond = theNth 2
/// Partially applied theNth with N = 3
let theThird = theNth 3
/// Partially applied theNth with N = 4
let theFourth = theNth 4
/// Partially applied theNth with N = 5
let theFifth = theNth 5
/// Partially applied theNth with N = 6
let theSixth = theNth 6
/// Partially applied theNth with N = 7
let theSeventh = theNth 7
/// Partially applied theNth with N = 8
let theEight = theNth 8
/// Partially applied theNth with N = 9
let theNinth = theNth 9
/// Partially applied theNth with N = 10
let theTenth = theNth 10

/// Class to provide methods that depend on a particular DateTimeZone to evaluate.
/// DateTimeZone is passed in the constructor and used in the resulting instance's methods.
/// A default implementation, LocalZoneDepends, is provided, which uses DateTimeZoneProviders.Bcl.GetSystemDefault().
type ZoneDependents(zone) =
    /// The DateTimeZone being used by this instance.
    member this.zone: DateTimeZone = zone

    /// Calculate the nth day of the week before or equal to the given instant in a time zone.
    /// For example, theFirst Tuesday |> beforeOrEqual (today ())
    /// will return the first Tuesday between 0 and 6 days ago.
    /// theSecond Tuesday |> beforeOrEqual (today ())
    /// will return one week before the first Tuesday between 0 and 6 days ago.
    member this.beforeOrEqual (instant: Instant) nthDay =
        let initialDay =
            instant.InZone(this.zone).DayOfWeek |> unbox<int>

        let targetDay = nthDay.day |> unbox<int>

        let dayDifference = (initialDay - targetDay + 7) % 7
        let weekDifference = nthDay.n - 1

        (weekDifference |> weeks)
        + (dayDifference |> days)
        |> before instant

    /// Calculate the nth day of the week before the given instant in a time zone.
    /// For example, theFirst Tuesday |> before (today ())
    /// will return the first Tuesday between 1 and 7 days ago.
    /// theSecond Tuesday |> before (today ())
    /// will return one week before the first Tuesday between 1 and 7 days ago.
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
