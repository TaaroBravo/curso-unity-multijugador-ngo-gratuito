# Unity Multiplayer con Netcode for GameObjects - Curso Gratuito

Proyecto del curso gratuito de **Unity Multiplayer con Netcode for GameObjects** de [Codearte](https://codearte.com.ar).

## Que contiene

Un **shooter top-down multiplayer** construido con Unity 6 y Netcode for GameObjects (NGO). El proyecto cubre los fundamentos de networking en Unity: conexion Host/Client, sincronizacion de movimiento, authority, RPCs y disparo networkeado.

### Temas cubiertos

- **NetworkManager** - Configuracion y conexion Host/Client
- **NetworkObject y NetworkBehaviour** - Objetos de red y logica multiplayer
- **NetworkTransform** - Sincronizacion de movimiento con Owner Authority
- **RPCs (ServerRpc / ClientRpc)** - Comunicacion cliente-servidor para el sistema de disparo
- **Spawn/Despawn** - Instanciar y destruir objetos en red (balas)
- **Scene Management** - Carga de escenas sincronizada

## Branches

| Branch | Descripcion |
|--------|-------------|
| **`main`** | Proyecto terminado con todo el networking implementado |
| **`proyecto-singleplayer`** | Proyecto singleplayer base, sin nada de networking. Usa este branch si queres seguir el curso desde cero |

## Requisitos

- **Unity 6** (6000.x)
- **Netcode for GameObjects** 2.10+

## Links

- [Codearte](https://codearte.com.ar) - Curso completo y mas recursos
- [Documentacion de NGO](https://docs-multiplayer.unity3d.com/netcode/current/about/)
