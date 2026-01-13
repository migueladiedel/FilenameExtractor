# Filename Extractor - Visual Studio 2022 Extension

Una extensión para Visual Studio 2022 que permite copiar el nombre de un archivo **sin su extensión** al portapapeles con un solo clic.

---

## Descripción

**Filename Extractor** agrega una nueva opción en los menús contextuales de Visual Studio que te permite copiar rápidamente el nombre de un archivo sin incluir la extensión. Esta funcionalidad es útil cuando necesitas:

- Referenciar nombres de clases (que suelen coincidir con el nombre del archivo)
- Crear variables o constantes basadas en nombres de archivos
- Documentar código sin incluir extensiones
- Cualquier situación donde necesites el nombre limpio del archivo

### Ejemplo

| Archivo original | Resultado copiado |
|------------------|-------------------|
| `Program.cs` | `Program` |
| `UserController.cs` | `UserController` |
| `appsettings.json` | `appsettings` |
| `config.development.json` | `config.development` |
| `Dockerfile` | `Dockerfile` |

---

## Características

- **Menú contextual en pestañas**: Clic derecho en la pestaña de cualquier archivo abierto
- **Menú contextual en Explorador de Soluciones**: Clic derecho en cualquier archivo del proyecto
- **Atajo de teclado**: `Ctrl+K, Ctrl+N` para copiar rápidamente
- **Multiidioma**: Interfaz disponible en inglés y español (detecta automáticamente el idioma de VS)
- **Confirmación visual**: Muestra el nombre copiado en la barra de estado

---

## Requisitos

- **Visual Studio 2022** (versión 17.0 o superior)
- Ediciones soportadas:
  - Community
  - Professional
  - Enterprise
- **.NET Framework 4.7.2** o superior

---

## Instalación

### Opción 1: Desde archivo VSIX (Recomendado)

1. Descarga el archivo `FilenameExtractorVSIX.vsix` desde la carpeta `bin/Release/`
2. Cierra todas las instancias de Visual Studio
3. Haz doble clic en el archivo `.vsix`
4. Sigue las instrucciones del instalador de VSIX
5. Abre Visual Studio 2022

### Opción 2: Compilar desde código fuente

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/tu-usuario/FilenameExtractor.git
   cd FilenameExtractor/FilenameExtractorVSIX
   ```

2. **Abrir en Visual Studio 2022**
   - Abre `FilenameExtractorVSIX.sln`

3. **Compilar**
   - Menú `Build` → `Rebuild Solution`
   - O presiona `Ctrl+Shift+B`

4. **Instalar**
   - Navega a `bin/Debug/` o `bin/Release/`
   - Ejecuta `FilenameExtractorVSIX.vsix`

### Opción 3: Depuración (para desarrolladores)

1. Abre la solución en Visual Studio 2022
2. Presiona `F5` para iniciar la depuración
3. Se abrirá una **instancia experimental** de Visual Studio
4. Prueba la extensión en esa instancia

---

## Uso

### Desde la pestaña del documento

1. Abre cualquier archivo en el editor
2. Haz **clic derecho** en la pestaña del archivo (donde aparece el nombre)
3. Selecciona **"Copy Filename Without Extension"** (o "Copiar nombre sin extensión" en español)
4. El nombre se copia al portapapeles

### Desde el Explorador de Soluciones

1. En el **Explorador de Soluciones**, localiza el archivo
2. Haz **clic derecho** sobre el archivo
3. Selecciona **"Copy Filename Without Extension"**
4. El nombre se copia al portapapeles

### Con atajo de teclado

1. Selecciona un archivo en el Explorador de Soluciones, o ten un archivo abierto
2. Presiona **`Ctrl+K`**, luego **`Ctrl+N`**
3. El nombre se copia al portapapeles

### Confirmación

Después de copiar, verás un mensaje en la **barra de estado** de Visual Studio:
```
Copiado: NombreDelArchivo
```

---

## Localización (Idiomas)

La extensión detecta automáticamente el idioma de Visual Studio y muestra los textos en el idioma correspondiente.

### Idiomas disponibles

| Idioma | Texto del menú |
|--------|----------------|
| Inglés (por defecto) | Copy Filename Without Extension |
| Español | Copiar nombre sin extensión |

### Agregar un nuevo idioma

Para agregar soporte para un nuevo idioma:

1. Copia el archivo `VSPackage.resx` y renómbralo como `VSPackage.XX.resx` (donde XX es el código de idioma, ej: `fr`, `de`, `pt`)

2. Traduce los valores en el nuevo archivo:
   ```xml
   <data name="100" xml:space="preserve">
     <value>TU TRADUCCIÓN AQUÍ</value>
   </data>
   ```

3. Recompila el proyecto

### Estructura de recursos

| Clave | Descripción |
|-------|-------------|
| `100` | Texto del comando en el menú |
| `StatusCopied` | Mensaje de barra de estado: "Copiado: {0}" |
| `ErrorNoFileSelected` | Error: ningún archivo seleccionado |
| `ErrorCopyFailed` | Error: fallo al copiar |
| `ErrorTitle` | Título de diálogos de error |

---

## Desinstalación

1. Abre Visual Studio 2022
2. Ve a `Extensions` → `Manage Extensions`
3. Busca **"Filename Extractor"**
4. Haz clic en **"Uninstall"**
5. Reinicia Visual Studio

---

## Estructura del Proyecto

```
FilenameExtractorVSIX/
├── Commands/
│   └── CopyFilenameWithoutExtensionCommand.cs  # Lógica del comando
├── Properties/
│   └── AssemblyInfo.cs                         # Metadatos del ensamblado
├── FilenameExtractorVSIX.csproj                # Archivo de proyecto
├── FilenameExtractorVSIX.sln                   # Archivo de solución
├── FilenameExtractorVSIXPackage.cs             # Clase principal del paquete
├── FilenameExtractorVSIXPackage.vsct           # Definición de comandos/menús
├── source.extension.vsixmanifest               # Manifiesto de la extensión
├── VSPackage.resx                              # Recursos (inglés)
├── VSPackage.es.resx                           # Recursos (español)
└── README.md                                   # Este archivo
```

---

## Desarrollo

### Requisitos para desarrollar

- Visual Studio 2022 con la carga de trabajo **"Visual Studio extension development"**
- .NET Framework 4.7.2 SDK

### Compilar en modo Release

```bash
msbuild FilenameExtractorVSIX.sln /p:Configuration=Release
```

### Ejecutar pruebas

1. Presiona `F5` en Visual Studio
2. En la instancia experimental:
   - Abre un proyecto
   - Prueba el comando desde la pestaña del documento
   - Prueba el comando desde el Explorador de Soluciones
   - Prueba el atajo `Ctrl+K, Ctrl+N`

---

## Solución de Problemas

### El comando no aparece en el menú

1. Verifica que estás usando Visual Studio 2022 (v17.x)
2. Asegúrate de que la extensión está instalada: `Extensions` → `Manage Extensions`
3. Intenta reinstalar la extensión

### El atajo de teclado no funciona

1. Ve a `Tools` → `Options` → `Environment` → `Keyboard`
2. Busca `FilenameExtractor.CopyFilenameWithoutExtension`
3. Verifica o reasigna el atajo

### Error al copiar

- Asegúrate de tener un archivo seleccionado o abierto
- Verifica los permisos del portapapeles del sistema

---

## Licencia

Este proyecto está bajo la licencia MIT. Ver el archivo `LICENSE` para más detalles.

---

## Contribuir

Las contribuciones son bienvenidas. Por favor:

1. Haz fork del repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Haz commit de tus cambios (`git commit -am 'Añadir nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request

---

## Changelog

### v1.0.0
- Versión inicial
- Comando en menú contextual de pestañas de documentos
- Comando en menú contextual del Explorador de Soluciones
- Atajo de teclado `Ctrl+K, Ctrl+N`
- Soporte para inglés y español
