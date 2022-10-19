
# DetailTECMobile

## Requisitos para ejecutar el proyecto

Utilizar Visual Studio 2019, este es un proyecto creado utilizando Xamarin, por lo que su instalación de Visual Studio debe de incluir la paqueteria necesaria para desarrollo de aplicaciones mobiles. Para verificar, puede seguir estos pasos:

- Ejecute Visual Studio Installer, puede obtener Visual Studio Installer en el siguiente [enlace](https://visualstudio.microsoft.com/es/vs/older-downloads/)
- Descargue la version de visual studio que se ajuste a su equipo desde el instalador. Al dar click en el boton instalar en la version deseada, vera un menu como el que se muestra a continuacion 


![image](https://user-images.githubusercontent.com/25675816/196600456-fcc9ff6c-e8b1-4c0a-b036-415517eaed89.png)


- Asegurese de seleccionar las opciones:
-  - ASP.NET and Web Development
-  - Mobile development with .NET

- De click en el boton instalar y espere a que termine el proceso y se ejecute Visual Studio.

Si ya tiene instalado Visual Studio y las dependencias necesarias, basta con clonar este repositorio a su equipo y abrir el .sln del proyecto con VS 2019.

![image](https://user-images.githubusercontent.com/25675816/196600891-14ff9e62-d826-4955-8c5a-b1decf894eb2.png)
![image](https://user-images.githubusercontent.com/25675816/196601072-4bc6b98a-8239-473b-9c47-ef6bfd784cdd.png)

----------------------------------------------------------

Vista default del proyecto

![image](https://user-images.githubusercontent.com/25675816/196602352-ba8bb751-b36e-4af3-9414-ee8d314de8b6.png)

Cree un emulador de android para pruebas y debugging:

![image](https://user-images.githubusercontent.com/25675816/196602499-278b570f-6ecd-49a9-ba2c-90cef073573a.png)
![image](https://user-images.githubusercontent.com/25675816/196602584-2a54f2a5-70f2-4c18-9dcc-8ee136d6ddff.png)

Use las opciones por defecto y de click en create
![image](https://user-images.githubusercontent.com/25675816/196602644-15c6d1ec-d0be-4f5f-89e3-b11eb8dfa5e2.png)

El nuevo emulador se vera listado en el device manager. Puede cerrar la ventana. Seguidamente haga click en ejecutar para probar el codigo

![image](https://user-images.githubusercontent.com/25675816/196603045-fc302508-1794-49b4-b834-ab514fe8f90e.png)

NOTA: El build puede durar varios minutos, dependiendo de su equipo. El proyecto esta configurado para que una vez que ejecute el simulador de android, no tenga que volver a hacerlo al cambiar el codigo fuente, por lo que se recomienda ejecutar una sola vez por sesion y no cerrar el emulador, los cambios deben de poder ser visualizados en tiempo real.

![image](https://user-images.githubusercontent.com/25675816/196603608-8ad908f5-54dc-42f2-9535-b509c30b4c65.png)

PARA USUARIOS DE WINDOWS:

Si al tratar de ejecutar el proyecto, el emulador de Android sale con una pantalla en negro y al presionar el boton de encendido no hay cambios, asegurese que HyperV este habilitado en su sistema, para mas informacion sobre esto visite los siguientes enlaces:
https://learn.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/


https://learn.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/hardware-acceleration?pivots=windows
