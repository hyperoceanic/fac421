namespace HTMX

open Falco.Markup

module HX =

    let get value =
        Attr.create "hx-get" value

    let target value =
        Attr.create "hx-target" value

    let swap value =
        Attr.create "hx-swap" value