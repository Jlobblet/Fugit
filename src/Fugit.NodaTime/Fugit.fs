[<AutoOpen>]
module Fugit.NodaTime.Fugit

open NodaTime
// TimeSpan constructors

/// Construct a Duration with a duration of ticks passed as argument.
/// Equivalent to Duration.FromTicks(numberTicks)
let ticks (numberTicks: int64) = Duration.FromTicks(numberTicks)

/// Construct a Duration with a duration of milliseconds passed as argument.
/// Equivalent to Duration.FromMilliseconds(numberMilliseconds)
let milliseconds (numberMilliseconds: int64) =
    Duration.FromMilliseconds(numberMilliseconds)

/// Construct a Duration with a duration of seconds passed as argument.
/// Equivalent to Duration.FromSeconds(numberSeconds)
let seconds (numberSeconds: int64) = Duration.FromSeconds(numberSeconds)

/// Construct a Duration with a duration of minutes passed as argument.
/// Equivalent to Duration.FromMinutes(numberMinutes)
let minutes (numberMinutes: int64) = Duration.FromMinutes(numberMinutes)

/// Construct a Duration with a duration of hours passed as argument.
/// Equivalent to Duration.FromHours(numberHours)
let hours (numberHours: int) = Duration.FromHours(numberHours)

/// Construct a Duration with a duration of days passed as argument.
/// Equivalent to Duration.FromDays(numberDays)
let days (numberDays: int) = Duration.FromDays(numberDays)

/// Construct a Duration with a duration of weeks passed as argument.
/// Equivalent to Duration.FromDays(7 * numberWeeks)
let weeks numberWeeks = Duration.FromDays(7 * numberWeeks)

// Addition and subtraction

/// Subtract a Duration from an Instant
let before (instant: Instant) (duration: Duration) = instant - duration

/// Add a Duration to an Instant
let after (instant: Instant) (duration: Duration) = instant + duration

// Boolean Comparisons

/// Return whether i1 - the second parameter - is before (less than) i2
let isBefore (i2: Instant) i1 = i1 < i2

/// Return whether i1 - the second parameter - is after (greater than) i2
let isAfter (i2: Instant) i1 = i1 > i2

/// Return whether i1 - the second parameter - is before or equal to (less than or equal to) i2
let isBeforeOrEqual (i2: Instant) i1 = i1 <= i2

/// Return whether i1 - the second parameter - is after or equal to (greater than or equal to) i2
let isAfterOrEqual (i2: Instant) i1 = i1 >= i2

/// Construct an Interval, with Interval.Start = start and Interval.End = end'.
let and' end' start = Interval(start, end')

/// Discriminated union used to make sure you use "every"
type Frequency = Frequency of Duration

/// Construct a Frequency, a single case discriminated union of Duration.
let every = Frequency

/// Create equally spaced Instants between interval.Start and interval.End, separated by freq.
/// Example usage: every day |> between (January 1 2000 |> and' (January 31 2000))
let between (interval: Interval) (Frequency freq) =
    Seq.unfold (fun c -> if c |> isAfter interval.End then None else Some(c, freq |> after c)) interval.Start

/// Wrapper for between, intended to be used with theNext and theLast
/// Example: every (5 |> minutes) |> in' (theLast hour)
let in' = between

/// Return whether an Instant is after interval.Start and before interval.End, inclusive of the boundaries.
let isBetween (interval: Interval) instant =
    instant |> isAfterOrEqual interval.Start
    && instant |> isBeforeOrEqual interval.End

// Days

type DayOfWeek =
    | Monday
    | Tuesday
    | Wednesday
    | Thursday
    | Friday
    | Saturday
    | Sunday
    static member toIsoDayOfWeek =
        function
        | Monday -> IsoDayOfWeek.Monday
        | Tuesday -> IsoDayOfWeek.Tuesday
        | Wednesday -> IsoDayOfWeek.Wednesday
        | Thursday -> IsoDayOfWeek.Thursday
        | Friday -> IsoDayOfWeek.Friday
        | Saturday -> IsoDayOfWeek.Saturday
        | Sunday -> IsoDayOfWeek.Sunday

    member this.IsoDayOfWeek = DayOfWeek.toIsoDayOfWeek this

type Placeholder = { n: int; day: DayOfWeek }

// Clock Dependent words

/// Class to provide methods that depend on the clock to determine.
/// Clock is passed in the constructor, and used in the methods.
/// A default implementation, SystemClockDependents, which uses SystemClock.Instance is provided.
type ClockDependents(clock) =
    /// The IClock being used by this instance
    member this.clock: IClock = clock
    /// Wrapper for this.clock.GetCurrentInstant
    member this.now() = this.clock.GetCurrentInstant()
    /// Equivalent to before (this.now())
    member this.ago = before (this.now ())
    /// Equivalent to freq |> between (then' and (this.now()))
    member this.since then' freq =
        freq |> between (then' |> and' (this.now ()))
    /// Construct an Interval starting now and ending duration after now
    member this.theNext duration =
        Interval(this.now (), duration |> after (this.now ()))
    /// Construct an Interval starting duration before now and ending now
    member this.theLast duration =
        Interval(duration |> before (this.now ()), this.now ())

// Defaults use the system clock, but allows for dependency injection
let SystemClockDependents = ClockDependents(SystemClock.Instance)
/// ClockDependents.now, with the SystemClock
let now = SystemClockDependents.now

/// ClockDependents.agp, with the SystemClock
let ago = SystemClockDependents.ago

/// ClockDependents.since, with the SystemClock
let since = SystemClockDependents.since

/// ClockDependents.theNext, with the SystemClock
let theNext = SystemClockDependents.theNext

/// ClockDependents.theLast, with the SystemClock
let theLast = SystemClockDependents.theLast
