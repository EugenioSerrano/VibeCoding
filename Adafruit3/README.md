# AdafruitIoClient

Esta solución contiene dos proyectos:

- AdafruitIoApiClient: Class Library (.NET 9) que encapsula la lógica de acceso a la API HTTP de Adafruit IO.
- AdafruitIoConsoleApp: Aplicación de consola (.NET 9) que utiliza la librería para interactuar con dashboards y feeds.

## Estructura

- La librería expone métodos para:
  - Listar dashboards y sus bloques.
  - Listar feeds asociados a un dashboard.
  - Consultar el estado actual de un feed.
  - Modificar el valor de un feed.

- La consola permite:
  - Listar componentes (feeds) de un dashboard.
  - Consultar el estado de un feed.
  - Modificar el valor de un feed.

## Configuración

Debes proveer tu API Key y username de Adafruit IO para que la aplicación funcione correctamente.

---

Este README se irá actualizando conforme avance la implementación.
