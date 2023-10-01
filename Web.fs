module html

open Falco.Markup

let home =
    Elem.html [ Attr.lang "en" ] [
        Elem.head [] []
        Elem.body [] [
            Elem.h1 [] [ Text.raw "Fac 421" ]
        ]
    ]