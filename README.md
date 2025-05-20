# BD_Proyecto

Carla De La Loma, Guillermo Torres, Guillermo Salicrú, Alejandro Nuñez.

## Diagrama E-R

![Diagrama E-R](https://github.com/user-attachments/assets/97c1c299-c74f-416b-a4cd-0ac0fdb38acc)

Nuestro diagrama consta de cinco entidades inspiradas en el juego de Among Us. La mayoría presentan una relación uno-a-muchos, ya que cada entidad principal está vinculada a múltiples elementos relacionados.

Por ejemplo, una sala puede contener varias tares, pero cada tarea pertenece a una sola sala. Del mismo modo, un rol puede asignarse a múltiples personajes, pero cada personaje solo tiene un rol específico.

## Tablas de la base de datos relacional

![image](https://github.com/user-attachments/assets/62b4490e-2736-427e-adbd-814b98c1af3e)

Así serían nuestras tablas relaciones. Basándonos en el diagrama E-R, hemos añadido las claves ajenas a las tablas correspondientes dependiendo de la relación que tenían. 

Por ejemplo, en la tabla "MUERTES", cada fila de la tabla representa una muerte ocurrida en el juego, especificando quién fue la víctima, quién fue el asesino y en qué sala ocurrió; en la tabla "TAREAS", cada tarea está ubicada en una sala y puede ser realizada por varios personajes, pero con un determinado rol.

La tabla "SALAS-SALAS" es una relación recursiva muchos-a-muchos, una sala puede estar conectada con múltiples salas diferentes que a su vez también esté conectada a otras salas.
