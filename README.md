# EN:
When player joins to the server, plugin replaces ads in his nickname with the specified text or just ban him.

## Compatible versions of SMod
3.4.0 - 3.5.1-K(tester ver.)

## How to install
Drop NoNickAds.dll in to sm_plugins.

## Configs
Value Type|Config Name|Default value|Description|
----------|-------------|-----------|-----------|
bool|NNA_DISABLED|false|Turns off the plugin
int|NNA_MODE|1|Plugin mode. If 1 - replaces nickname, if 2 - ban the player
list|NNA_BLACKLISTED_WORDS||List of advertisements/banned nicknames. Case are ignored*.
string|NNA_TEXT||Replaces the advertisement with the specified text. It's recommended to leave the field blank. If NNA_MODE = 2, it causes the ban. Use %word% to specify the word that caused the ban.
int|NNA_DURATION|2|Ban's duration in minutes. 525600 - 1 year, 26280000 - 50 years
bool|NNA_SELECTIVE_DELETION|true|Selective deletion of ads. If true - only deletes what is specified in NNA_BLACKLISTED_WORDS, if false - completely deletes the word. [If true](https://cdn.discordapp.com/attachments/595913512065171467/640550217808085002/unknown1.png), [If false](https://cdn.discordapp.com/attachments/595913512065171467/640550223730442260/unknown2.png).
list|NNA_WHITELIST||A list of steamid64 of players who are allowed to bypass the check

### NNA_BLACKLISTED_WORDS Example
NNA_BLACKLISTED_WORDS: SoMеS1TеHeRe.com,someshittyad,FrеeCsgоskins.org,D0TАSKINS4FRЕE,pubgitеms.com,etc...

### NNA_WHITELIST Example
NNA_WHITELIST: 76561198111120799,76561198xxxxxxxxx,76561197xxxxxxxxx,76561752xxxxxxxxx,etc...

*Word "example" for plugin as the same as "EXamPle" or "EXAMPLE"

***

# RU:
При входе на сервер игрока, плагин заменяет рекламу в его нике указанным текстом или просто банит его.

## Совместимые версии SMod'а
3.4.0 - 3.5.1-K(тестовая версия)

## Установка
Перекиньте файл NoNickAds.dll в папку sm_plugins.

## Конфиги
Тип данных|Конфиг|Значение по-умолчанию|Описание|
----------|-------------|-----------|-----------|
bool|NNA_DISABLED|false|Выключает плагин
int|NNA_MODE|1|Режим плагина. Если 1 - заменяет ник, если 2 - банит игрока
list|NNA_BLACKLISTED_WORDS||Список рекламы/запрещённых ников. Регистр не учитывается*.
string|NNA_TEXT||Заменяет рекламу указанным текстом. Рекомендуется оставить поле пустым. Если NNA_MODE = 2, то является причиной бана. Используйте %word% чтобы указать слово, из-за которого игрок получил бан.
int|NNA_DURATION|2|Продолжительность бана в минутах. 525600 - 1 год, 26280000 - 50 лет.
bool|NNA_SELECTIVE_DELETION|true|Выборочное удаление рекламы. Если true - удаляет только то, что указано в NNA_BLACKLISTED_WORDS, если false - удаляет слово полностью. [If true](https://cdn.discordapp.com/attachments/595913512065171467/640550217808085002/unknown1.png), [If false](https://cdn.discordapp.com/attachments/595913512065171467/640550223730442260/unknown2.png).
list|NNA_WHITELIST||Список steamid64 игроков, которые могут обойти проверку

### NNA_BLACKLISTED_WORDS Пример
NNA_BLACKLISTED_WORDS: SoMеS1TеHeRe.com,someshittyad,FrеeCsgоskins.org,D0TАSKINS4FRЕE,pubgitеms.com,etc...

### NNA_WHITELIST Пример
NNA_WHITELIST: 76561198111120799,76561198xxxxxxxxx,76561197xxxxxxxxx,76561752xxxxxxxxx,etc...

*Для плагина нет разницы между "пример", "ПРиМер" и "ПРИМЕР"
