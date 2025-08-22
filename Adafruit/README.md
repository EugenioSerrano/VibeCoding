# Adafruit IO .NET 9 Solution

Esta solución contiene dos proyectos:

- **AdafruitIoClient**: Class Library que encapsula la lógica de acceso a la API de Adafruit IO.
- **AdafruitIoConsole**: Aplicación de consola que consume la librería y permite interactuar con dashboards y feeds.

## Configuración

1. Copia tu `username` y `AIO Key` en el archivo `AdafruitIoConsole/appsettings.json`.
2. Usa la consola para listar dashboards, feeds, consultar y modificar valores.

## Estructura

- Uso de `IHttpClientFactory` y DI para clientes HTTP robustos.
- Métodos asincrónicos y manejo de errores.
- Documentación y ayuda contextual con Context7.

## Próximos pasos
- Implementar la clase principal del cliente en `AdafruitIoClient`.
- Implementar la lógica de consola en `AdafruitIoConsole`.
