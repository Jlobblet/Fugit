# Fugit

An F# library for syntactically nice construction of DateTime and TimeSpan objects.  
_Tempus fugit._

## Overview

Fugit provides functions that make code dealing with DateTime and TimeSpan objects nicer to work with.
```f#
oneDay |> ago
2 |> weeks |> before (January 23 2021)
every oneHour |> since (5 |> hours |> ago)
// Or, equivalently
every oneHour |> in' (theLast (5 |> hours))
if input |> isAfter (52 |> weeks |> ago) then 
    // Relative days of the week
    theThird Monday |> after (today())
```

## Installation

Fugit is available on NuGet at [https://www.nuget.org/packages/Fugit/](https://www.nuget.org/packages/Fugit/).  
Fugit.NodaTime is available on NuGet at [https://www.nuget.org/packages/Fugit.NodaTime/](https://www.nuget.org/packages/Fugit/).

## Documentation

Proper documentation is in progress!
In the meantime, most of the code is documented and fairly straightforward.

## Differences between Fugit and Fugit.NodaTime

While I have strived my best to make the two libraries as similar as possible, there are a few differences:

- Some Fugit.NodaTime constructors use `int64`, whereas all Fugit constructors use `int`
- Fugit.NodaTime allows dependency injection on functions that rely on the current time, Fugit does not.


## Contributing

Pull requests are very welcome!
Code is formatted with [Fantomas](https://github.com/fsprojects/fantomas).

## License

Fugit is licensed under the GPL-3.0 license. You can read it [here](LICENSE).
