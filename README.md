# EN:
When player joins to the server, plugin replaces ads in his nickname with the specified text or just ban him.

## How to install
Drop NoNickAds.dll in to Plugins.

## Configs
Value Type|Config Name|Default value|Description|Variables|
----------|-------------|-----------|-----------|---------|
bool|nna_disable|false|Turns off the plugin
int|nna_mode|1|Plugin mode. 1 — replace mode, 2 — ban mode|
list|nna_blacklisted_words||List of advertisements/banned nicknames. Use `,` as separator. Case are ignored*.|
string|nna_text||Replaces the advertisement with the specified text. It's recommended to leave the field blank. If nna_mode = 2, it causes the ban. Use %words% to substitute the words that caused ban. |%words%
int|nna_ban_duration|2|Ban duration in minutes. 525600 — 1 year, 26280000 — 50 years|
bool|nna_selective_replacing|true|Selective replacing of ads. If true - only replaces what is specified in nna_blacklisted_words, if false - completely replaces the word. [If true](https://cdn.discordapp.com/attachments/595913512065171467/640550217808085002/unknown1.png), [If false](https://cdn.discordapp.com/attachments/595913512065171467/640550223730442260/unknown2.png).|
list|nna_whitelisted_players||A list of userid of players who are allowed to bypass the check. Use `,` as separator|
bool|nna_use_unicode_normalization|true|Read [this](https://unicode.org/reports/tr15/)|
special format|nna_unicode_normalization|FormC|FormC, FormD, FormKC or FormKD|
string|nna_custom_regex||Default regex not bad. Сhange it if it's really necessary and you know what are you doing. Custom regex override selective replacing, but not site replacer. [Good luck, have fun](https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference).|{word}
bool|nna_use_smart_site_replacer|true|Really powerful thing that can eliminate site ads in player's nick.|

### nna_blacklisted_words Example
nna_blacklisted_words: SoMеS1TеHeRe.com,someshittyad,FrеeCsgоskins.org,D0TАSKINS4FRЕE,pubgitеms.com,etc...

### nna_whitelisted_players Example
nna_whitelisted_players: 76561198111120799@steam,UnknownPole@northwood,242306234269696000@discord,slpatreon@patreon,etc...

*Word "example" for plugin as the same as "EXamPle" or "EXAMPLE"

***

# RU:
При входе на сервер игрока, плагин заменяет рекламу в его нике указанным текстом или просто банит его.

## Установка
Перекиньте файл NoNickAds.dll в папку Plugins.

## Конфиги
Тип данных|Конфиг|Значение по-умолчанию|Описание|Переменные|
----------|------|---------------------|--------|----------|
bool|nna_disable|false|Выключает плагин
int|nna_mode|1|Режим плагина. 1 — режим замены, 2 — режим бана|
list|nna_blacklisted_words||Список рекламы/запрещённых ников. Используйте `,` как разделитель. Регистр не учитывается*.|
string|nna_text||Заменяет рекламу указанным текстом. Рекомендуется оставить поле пустым. Если nna_mode = 2, то является причиной бана. Используйте %words%, чтобы подставить слова, которые привели к бану|%words%
int|nna_ban_duration|2|Продолжительность бана в минутах. 525600 — 1 год, 26280000 — 50 лет.|
bool|nna_selective_replacing|true|Выборочная замена рекламы. Если true - заменяет только то, что указано в nna_blacklisted_words, если false - заменяет слово полностью. [If true](https://cdn.discordapp.com/attachments/595913512065171467/640550217808085002/unknown1.png), [If false](https://cdn.discordapp.com/attachments/595913512065171467/640550223730442260/unknown2.png).|
list|nna_whitelisted_players||Список userid игроков, которые могут обойти плагин. Используйте `,` как разделитель.|
bool|nna_use_unicode_normalization|true|Прочитайте [вот это(многабукаф на английском)](https://unicode.org/reports/tr15/)|
special format|nna_unicode_normalization|FormC|FormC, FormD, FormKC или FormKD|
string|nna_custom_regex||Regex, что стоит по-умолчанию, неплох. Изменяйте его только в том случае, если вы понимаете, что делаете. Кастомный regex перекрывает selective replacing, но не site replacer. [Удачи](https://docs.microsoft.com/ru-ru/dotnet/standard/base-types/regular-expression-language-quick-reference).|{word}
bool|nna_use_smart_site_replacer|true|Очень мощная штука, которая способна истребить всю рекламу сайтов из ника игрока.|

### nna_blacklisted_words Пример
nna_blacklisted_words: SoMеS1TеHeRe.com,someshittyad,FrеeCsgоskins.org,D0TАSKINS4FRЕE,pubgitеms.com,etc...

### nna_whitelisted_players Пример
nna_whitelisted_players: 76561198111120799@steam,UnknownPole@northwood,242306234269696000@discord,slpatreon@patreon,etc...

*Для плагина нет разницы между "пример", "ПРиМер" и "ПРИМЕР"
