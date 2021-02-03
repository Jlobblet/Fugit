[<RequireQualifiedAccess>]
module Fugit.DayOfWeek

open System
open Fugit.Shorthand

/// Use a function from Fugit.DayOfWeek to construct this record.
/// tryTheNth, theNth, theFirst (etc.)
///
/// Contains a day of the week and a number of weeks to add or subtract.
type TheNthDay = { n: int; day: DayOfWeek }

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

/// Calculate the nth day of the week before or equal to the given date.
/// For example, theFirst Tuesday |> beforeOrEqual (today ())
/// will return the first Tuesday between 0 and 6 days ago.
/// theSecond Tuesday |> beforeOrEqual (today ())
/// will return one week before the first Tuesday between 0 and 6 days ago.
let beforeOrEqual (date: DateTime) nthDay =
    let initialDay = date.DayOfWeek |> unbox<int>
    let targetDay = nthDay.day |> unbox<int>

    let dayDifference = (initialDay - targetDay + 7) % 7
    let weekDifference = nthDay.n - 1

    (weekDifference |> weeks)
    + (dayDifference |> days)
    |> before date

/// Calculate the nth day of the week before the given date.
/// For example, theFirst Tuesday |> before (today ())
/// will return the first Tuesday between 1 and 7 days ago.
/// theSecond Tuesday |> before (today ())
/// will return one week before the first Tuesday between 1 and 7 days ago.
let before (date: DateTime) nthDay =
    beforeOrEqual (oneDay |> before date) nthDay

/// Calculate the nth day of the week after or equal to the given date.
/// For example, theFirst Tuesday |> afterOrEqual (today ())
/// will return the first Tuesday between 0 and 6 days in the future.
/// theSecond Tuesday |> beforeOrEqual (today ())
/// will return one week after the first Tuesday between 0 and 6 days in the future.
let afterOrEqual (date: DateTime) nthDay =
    let initialDay = date.DayOfWeek |> unbox<int>
    let targetDay = nthDay.day |> unbox<int>

    let dayDifference = (targetDay - initialDay + 7) % 7
    let weekDifference = nthDay.n - 1

    (weekDifference |> weeks)
    + (dayDifference |> days)
    |> after date

/// Calculate the nth day of the week after to the given date.
/// For example, theFirst Tuesday |> afterOrEqual (today ())
/// will return the first Tuesday between 1 and 7 days in the future.
/// theSecond Tuesday |> beforeOrEqual (today ())
/// will return one week after the first Tuesday between 1 and 7 days in the future.
let after (date: DateTime) nthDay =
    afterOrEqual (oneDay |> after date) nthDay
