# RickAndMortyBff

API en .NET 8 que funciona como Backend for Frontend (BFF) para la aplicación de Rick and Morty. Su trabajo es consumir la API pública de Rick and Morty (https://rickandmortyapi.com/) y exponer al frontend una versión adaptada de los datos: limpia, tipada y pensada para lo que la interfaz necesita mostrar.

El frontend nunca habla directo con la API externa. Conversa con el backend, el cual centraliza el consumo, transforma las respuestas y maneja los errores en un solo lugar.

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (el proyecto fija esta versión mediante `global.json`)
- Visual Studio 2022 o cualquier editor con soporte para .NET

## Cómo levantar la API

Desde la carpeta del proyecto:

```bash
cd RickAndMortyBff
dotnet run
```

O bien, abre `RickAndMortyBff.slnx` en Visual Studio 2026 y ejecuta con F5.

La API quedó disponible en mi equipo con el puerto `https://localhost:7088`. En modo desarrollo se habilita Swagger para explorar y probar los endpoints:

```
https://localhost:7088/swagger
```

## Configuración

La URL de la API externa no está escrita en el código. Vive en `appsettings.json`, bajo la sección `RickAndMortyApi`:

```json
{
  "RickAndMortyApi": {
    "BaseUrl": "https://rickandmortyapi.com/api/"
  }
}
```

Esa configuración se lee de forma tipada mediante una clase de opciones, y se inyecta en el cliente HTTP.

## Endpoints

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/episodes?page={n}&name={texto}` | Lista episodios paginados. El parámetro `name` es opcional y filtra por nombre. |
| GET | `/api/episodes/{id}` | Detalle de un episodio, incluyendo los personajes que aparecen en él. |
| GET | `/api/characters/{id}` | Detalle de un personaje. |

## Cómo está organizado

El proyecto separa responsabilidades por capas, cada una con un rol claro:

- **Controllers** — reciben las peticiones HTTP y devuelven las respuestas. No contienen lógica de negocio; delegan en los servicios.
- **Services** — coordinan la lógica: piden datos al cliente, los transforman a los DTOs que consume el frontend y arman la paginación.
- **Infrastructure** — el cliente HTTP que habla con la API externa de Rick and Morty.
- **Models** — separados en dos grupos: los modelos `External`, que reflejan tal cual el JSON de la API externa, y los `Dtos`, que son los modelos limpios que la API expone al frontend.
- **Configuration** — la clase de opciones tipadas para la configuración.
- **Middleware** — el manejo centralizado de errores.

Las capas dependen de interfaces, no de implementaciones concretas, y se conectan mediante el contenedor de inyección de dependencias. Esto mantiene el código desacoplado y testeable.

## Manejo de errores

Un middleware envuelve todas las peticiones. Si algo falla (por ejemplo, la API externa no responde), lo captura, registra el detalle técnico en el log y devuelve al cliente una respuesta JSON limpia con el código de estado adecuado, sin filtrar detalles internos.

## Pruebas

El proyecto `RickAndMortyBff.Tests` contiene pruebas unitarias del servicio de episodios, usando xUnit y Moq para aislar las dependencias. Para ejecutarlas:

```bash
dotnet test
```

O desde el Explorador de pruebas de Visual Studio 2026.
