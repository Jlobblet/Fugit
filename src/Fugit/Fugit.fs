[<AutoOpen>]
module Fugit.Fugit
open System
// TimeSpan constructors

/// Construct a TimeSpan with a duration of ticks passed as argument.
/// Equivalent to TimeSpan(numberTicks)
let ticks numberTicks = TimeSpan(numberTicks)

/// Construct a TimeSpan with a duration of milliseconds passed as argument.
/// Equivalent to TimeSpan(0, 0, 0, 0, numberMilliseconds)
let milliseconds numberMilliseconds = TimeSpan(0, 0, 0, 0, numberMilliseconds)

/// Construct a TimeSpan with a duration of seconds passed as argument.
/// Equivalent to TimeSpan(0, 0, numberSeconds)
let seconds numberSeconds = TimeSpan(0, 0, numberSeconds)

/// Construct a TimeSpan with a duration of minutes passed as argument.
/// Equivalent to TimeSpan(0, numberMinutes, 0)
let minutes numberMinutes = TimeSpan(0, numberMinutes, 0)

/// Construct a TimeSpan with a duration of hours passed as argument.
/// Equivalent to TimeSpan(numberHours, 0, 0)
let hours numberHours = TimeSpan(numberHours, 0, 0)

/// Construct a TimeSpan with a duration of days passed as argument.
/// Equivalent to TimeSpan(numberDays, 0, 0, 0)
let days numberDays = TimeSpan(numberDays, 0, 0, 0)

/// Construct a TimeSpan with a duration of weeks passed as argument.
/// Equivalent to TimeSpan(7 * numberWeeks, 0, 0, 0)
let weeks numberWeeks = TimeSpan(7 * numberWeeks, 0, 0, 0)

// DateTime constructors

/// Wrapper for DateTime.Now
let now () = DateTime.Now

/// Wrapper for DateTime.UtcNow
let utcNow () = DateTime.UtcNow

/// Wrapper for DateTime.Today
let today () = DateTime.Today

// Addition and subtraction

/// Subtract a TimeSpan from a DateTime
let before (dateTime: DateTime) (timeSpan: TimeSpan) = dateTime - timeSpan

/// Subtract a TimeSpan from DateTime.Now
let ago = before (now ())

/// Add a TimeSpan to a DateTime
let after (dateTime: DateTime) (timeSpan: TimeSpan) = dateTime + timeSpan

// Boolean Comparisons

/// Return whether d1 - the second parameter - is before (less than) d2
let isBefore (d2: DateTime) d1 = d1 < d2

/// Return whether d1 - the second parameter - is after (greater than) d2
let isAfter (d2: DateTime) d1 = d1 > d2

/// Return whether d1 - the second parameter - is before or equal to (less than or equal to) d2
let isBeforeOrEqual (d2: DateTime) d1 = d1 <= d2

/// Return whether d1 - the second parameter - is after or equal to (greater than or equal to) d2
let isAfterOrEqual (d2: DateTime) d1 = d1 >= d2

/// Use Fugit.and' to construct this record.
///
/// Contains information about a period of time: its start and end'.
type PeriodBoundaries = { start: DateTime; end': DateTime }
/// Construct a PeriodBoundaries, a record which contains a start and end.
let and' end' start = { start = start; end' = end' }

/// Use Fugit.every to construct this discriminated union.
///
/// Single case discriminated union encoding a frequency.
type TimeSpanFrequency = Frequency of TimeSpan
/// Construct a TimeSpanFrequency, used to calculate equally spaced DateTimes
let every (timeSpan: TimeSpan) = Frequency timeSpan

/// Create equally spaced DateTimes between period.start and period.end', separated by freq.
/// Example usage: every day |> between (January 1 2000 |> and' (January 31 2000))
let between period (Frequency freq) =
    Seq.unfold (fun c -> if c |> isAfter period.end' then None else Some(c, freq |> after c)) period.start

/// Create equally spaced DateTimes between then' and now, separated by freq.
/// Equivalent to freq |> between (start and' (now())
let since then' freq =
    freq |> between (then' |> and' (now ()))

/// Return whether a DateTime is after period.start and before period.end', inclusive of the boundaries.
let isBetween period dateTime =
    dateTime |> isAfterOrEqual period.start
    && dateTime |> isBeforeOrEqual period.end'
