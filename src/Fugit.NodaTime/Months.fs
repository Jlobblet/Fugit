module Fugit.NodaTime.Months

open NodaTime

let private month m d y = LocalDate(y, m, d)

let January day year = month 1 day year
let January' year day = month 1 day year

let February day year = month 2 day year
let February' year day = month 2 day year

let March day year = month 3 day year
let March' year day = month 3 day year

let April day year = month 4 day year
let April' year day = month 4 day year

let May day year = month 5 day year
let May' year day = month 5 day year

let June day year = month 6 day year
let June' year day = month 6 day year

let July day year = month 7 day year
let July' year day = month 7 day year

let August day year = month 8 day year
let August' year day = month 8 day year

let September day year = month 9 day year
let September' year day = month 9 day year

let October day year = month 10 day year
let October' year day = month 10 day year

let November day year = month 11 day year
let November' year day = month 11 day year

let December day year = month 12 day year
let December' year day = month 12 day year
