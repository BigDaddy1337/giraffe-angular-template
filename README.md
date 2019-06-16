# F# Giraffe with Angular 8 Web Application

[Giraffe](https://github.com/giraffe-fsharp/Giraffe)
[Angular](https://github.com/angular/angular)

## Build and test the application

### Windows

Run the `build.bat` script in order to restore, build and test (if you've selected to include tests) the application:

```
> ./build.bat
```

### Linux/macOS

Run the `build.sh` script in order to restore, build and test (if you've selected to include tests) the application:

```
$ ./build.sh
```

### Docker

```
> docker-compose -f docker-compose.dev.yml up --build
```

## Run the application

After a successful build you can start the web application by executing the following command in your terminal:

```
> cd src
> dotnet run
```

After the application has started visit [http://localhost:5000](http://localhost:5000) in your preferred browser.