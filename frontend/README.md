# Explorador de Rick and Morty

Aplicación web realizada en Angular, que permite explorar los episodios de Rick and Morty, ver el detalle de cada uno con sus personajes, y navegar hasta la ficha de cada personaje. Consume los datos desde un backend propio (BFF en .NET 8), no directamente desde la API pública.

## Requisitos

- [Node.js](https://nodejs.org/) 20 o superior
- El backend (`RickAndMortyBff`) corriendo en `https://localhost:7088`

## Cómo levantar la web

Instala las dependencias la primera vez:

```bash
npm install
```

Y arranca el servidor de desarrollo:

```bash
npm start
```

La aplicación queda disponible en:

```
http://localhost:4200
```

> Importante: el backend tiene que estar corriendo en paralelo. Son dos servidores distintos. Si la web carga pero no muestra episodios, lo más probable es que el backend no esté levantado.

## Configuración

La URL del backend no está escrita en el código. Está en los archivos de entorno, dentro de `src/environments/`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7088/api'
};
```

## Qué se puede hacer

- **Listado de episodios** con paginación.
- **Búsqueda** de episodios por nombre, con un pequeño retardo para no disparar una petición por cada tecla, y un botón para limpiar el filtro.
- **Detalle de episodio**: muestra la información del episodio y la lista de personajes que aparecen, cada uno como un enlace.
- **Detalle de personaje**: ficha con su imagen, estado, especie, género, origen y ubicación.
- **Manejo de errores** visible: si una carga falla, se muestra un mensaje claro con opción de reintentar.

La navegación es de tres niveles: del listado entras a un episodio, del episodio a un personaje, y desde el personaje vuelves al punto anterior.

## Cómo está organizado

- **features/** — cada vista principal en su propia carpeta: `episode-list`, `episode-detail`, `character-detail`.
- **shared/** — componentes reutilizables entre vistas, como el mensaje de error.
- **services/** — la comunicación con el backend, separada de los componentes.
- **models/** — las interfaces TypeScript que tipean los datos. No se hace uso de `any`.

## Detalles técnicos

La aplicación usa varias de las capacidades recientes de Angular:

- **Signals** para el manejo de estado dentro de los componentes.
- **Standalone components**, sin módulos.
- **Detección de cambios sin Zone.js** (zoneless).
- **Nuevo control de flujo** en las plantillas (`@if`, `@for`, `@defer`).
- **Carga diferida** de la lista de personajes con `@defer`, junto con hidratación incremental sobre SSR.
- **inputs y outputs basados en signals** en el componente de error.

Los estilos están escritos en CSS puro, sin uso de frameworks. El tema visual se inspira en la estética del portal de la serie.
