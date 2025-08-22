# Adafruit IO Console App

Esta aplicación de consola en .NET 9 permite listar, consultar y comandar componentes (feeds) de Adafruit IO usando su API HTTP.

## Configuración

1. Copia el archivo `.env.example` a `.env` en la carpeta del proyecto.
2. Coloca tu usuario y tu API Key de Adafruit IO en el archivo `.env`:

```
ADAFRUIT_IO_USERNAME=tu_usuario
ADAFRUIT_IO_KEY=tu_api_key
```

## Uso

Ejecuta la app y sigue el menú interactivo:
- Listar feeds
- Consultar el estado de un feed
- Enviar comando a un feed

## Requisitos
- .NET 9 SDK
- Acceso a internet

## Notas
- La app usa peticiones HTTP estándar, no requiere SDK externo.
- Puedes ampliar la lógica para manejar grupos, dashboards, etc. según la documentación oficial.
