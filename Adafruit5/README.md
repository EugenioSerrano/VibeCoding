# Adafruit IO .NET Example

Este repositorio contiene dos proyectos:

- **AdafruitIoApi**: ClassLibrary que encapsula la lógica de acceso a la API HTTP de Adafruit IO.
- **AdafruitIoConsoleApp**: Aplicación de consola que usa la librería para obtener, consultar y modificar feeds de un dashboard.

## Uso rápido

1. Abre `Program.cs` en `AdafruitIoConsoleApp` y reemplaza `TU_USUARIO` y `TU_API_KEY` por tus datos de Adafruit IO.
2. Ejecuta la app:

    dotnet run --project .\AdafruitIoConsoleApp

3. Sigue las instrucciones en consola para listar dashboards, feeds, consultar y modificar valores.

## Documentación de la API
https://io.adafruit.com/api/docs/#adafruit-io-http-api

---

**Desarrollado con .NET 9.0**
