# Screen-ArkoPong #

## Instruction manual ##

* Download the latest version of the game [here][Releases] or build the project yourself.
* Download and run your [game server][Server];
* Configure connection parameters in the settings.json file. 

> If you build the project yourself, the `settings.json` file will be created during the first application run.

### .json arguments ###
| Argument    | Description                     | Example                             |
|-------------|---------------------------------|-------------------------------------|
| `phoneURL`  | Phone page URL.                 | `http://localhost:5000/index.html`  |
| `serverURL` | SignalR "pong" hub URL.         | `http://localhost:5000/pong`        |


[Server]: https://github.com/RTUITLab/arkopongBack
[Releases]: https://github.com/RTUITLab/Screen-ArkoPong/releases