


# Descripción del Script de Unity - AchievementsList

Este script de Unity está diseñado para gestionar los logros (achievements) en un juego utilizando Firebase Realtime Database. Permite desbloquear logros basados en estadísticas específicas de los jugadores, como victorias, derrotas, asesinatos, muertes, etc.

## Requisitos

- **Unity**: Este script está diseñado para ser utilizado en proyectos de Unity.
- **Firebase Realtime Database**: Se requiere una instancia de Firebase Realtime Database para almacenar y gestionar los datos de los jugadores y sus logros.

## Funcionamiento

1. **Inicialización**: En el método `Start()`, el script inicializa la conexión con Firebase Realtime Database y obtiene una referencia a la ubicación de los datos de los jugadores.

2. **Definición de Logros**: El script define una serie de logros en un diccionario, donde cada logro tiene un identificador único y está asociado a una estadística requerida para desbloquearlo.

3. **Consulta de Estadísticas**: En el método `Update()`, el script realiza consultas a la base de datos para obtener las estadísticas de los jugadores y determinar si se han cumplido los requisitos para desbloquear los logros.

4. **Desbloqueo de Logros**: Cuando se cumplen los requisitos para desbloquear un logro, el script actualiza la base de datos para marcar el logro como completado y muestra un mensaje de notificación en pantalla.

## Funciones Principales

- **`QueryAndUpdateAchievements(string key)`:** Consulta y actualiza el estado de un logro en la base de datos.
- **`UnlockAchievement(string key)`:** Desbloquea un logro y muestra una notificación en pantalla.
- **`QueryDatabaseAchievementName(string achievement, string name, Action<string> callback)`:** Consulta el nombre de un logro en la base de datos.
- **`QueryDatabaseAchievementCompleted(string achievement, Action<bool> callback)`:** Consulta el estado de completado de un logro en la base de datos.
- **`QueryDatabaseStats(string key, Action<int> callback)`:** Consulta las estadísticas de un jugador en la base de datos.

## Configuración Adicional

- **Idioma del Juego**: El script muestra mensajes de notificación en función del idioma configurado por el jugador en el juego.

---

