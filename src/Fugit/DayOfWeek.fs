[<RequireQualifiedAccess>]
module Fugit.DayOfWeek

open System
open Fugit.Shorthand

type TheNthDay = { n: int; day: DayOfWeek }

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

let beforeOrEqual (date: DateTime) nthDay =
    let initialDay = date.DayOfWeek |> unbox<int>
    let targetDay = nthDay.day |> unbox<int>

    let dayDifference = (initialDay - targetDay + 7) % 7
    let weekDifference = nthDay.n - 1

    (weekDifference |> weeks)
    + (dayDifference |> days)
    |> before date

let before (date: DateTime) nthDay =
    beforeOrEqual (oneDay |> before date) nthDay

let afterOrEqual (date: DateTime) nthDay =
    let initialDay = date.DayOfWeek |> unbox<int>
    let targetDay = nthDay.day |> unbox<int>

    let dayDifference = (targetDay - initialDay + 7) % 7
    let weekDifference = nthDay.n - 1

    (weekDifference |> weeks)
    + (dayDifference |> days)
    |> after date

let after (date: DateTime) nthDay =
    afterOrEqual (oneDay |> after date) nthDay
