module Views
    open Giraffe
    open GiraffeViewEngine

    let ngAppTag = tag "app-root"  [] []
      
    let ngScripts = 
      [ "main.js"; "runtime.js"; "polyfills.js" ]
      |> List.map (function js -> script [ _type "text/javascript"; _src js ] [] )
      
    let index = 
      html [ _lang "en" ] [
          head [] [
              meta [ _charset "utf-8"; _name "viewport"; _content "width=device-width, initial-scale=1" ] 
              title [] [ str "Giraffe Angular App" ]
              ``base`` [ _href "/" ]
              link [ _rel "icon"; _type "icon"; _href "favicon.ico" ]
              link [ _rel "stylesheet"; _href "styles.css" ]]
  
          body [] 
              (ngAppTag :: ngScripts)
      ]
