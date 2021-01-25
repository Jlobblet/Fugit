module Fugit.NodaTime.Shorthand

/// A TimeSpan with a duration of 1 millisecond.
/// Equivalent to TimeSpan(0, 0, 0, 1)
let millisecond = 1L |> milliseconds
/// A TimeSpan with a duration of 1 millisecond.
/// Equivalent to TimeSpan(0, 0, 0, 1)
let oneMillisecond = millisecond

/// A TimeSpan with a duration of 1 second.
/// Equivalent to TimeSpan(0, 0, 1)
let second = 1L |> seconds
/// A TimeSpan with a duration of 1 second.
/// Equivalent to TimeSpan(0, 0, 1)
let oneSecond = second

/// A TimeSpan with a duration of 1 minute.
/// Equivalent to TimeSpan(0, 1, 0)
let minute = 1L |> minutes
/// A TimeSpan with a duration of 1 minute.
/// Equivalent to TimeSpan(0, 1, 0)
let oneMinute = minute

/// A TimeSpan with a duration of 1 hour.
/// Equivalent to TimeSpan(numberHours, 0, 0)
let hour = 1 |> hours
/// A TimeSpan with a duration of 1 hour.
/// Equivalent to TimeSpan(numberHours, 0, 0)

let oneHour = hour

/// A TimeSpan with a duration of 1 day.
/// Equivalent to TimeSpan(numberDays, 0, 0, 0)
let day = 1 |> days
/// A TimeSpan with a duration of 1 day.
/// Equivalent to TimeSpan(numberDays, 0, 0, 0)

let oneDay = day

/// A TimeSpan with a duration of 7 days.
/// Equivalent to TimeSpan(7, 0, 0, 0)
let week = 7 |> days
/// A TimeSpan with a duration of 7 days.
/// Equivalent to TimeSpan(7, 0, 0, 0)
let oneWeek = week
