# EN [RU](https://github.com/Qidan475/NoNickAds#ru-en)
When player joins to the server, plugin replaces ads in his nickname with the specified text or just ban him.

## How to install
Drop NoNickAds.dll in to EXILED\Plugins.

## Config example
NoNickAds:  
  is_enabled: true  
  whitelisted_players: \[76561198111120799@steam, 242306234269696000@discord, UnknownPole@northwood, notahubert@patreon]  
  ads: \[#yracribniulr, example]  
  use_smart_link_replacer: true  
  replacement_text: ''  
  lite_replacing: true  
  banned_words: \[nigga, nigger]  
  ban_duration_in_minutes: 120  
  use_unicode_normalization: false  
  unicode_normalization: FormC  
  kick_msg_when_nick_is_empty: Looks like your nickname is all about advertising. Change it and re-join to the server.  
  ban_msg_when_nick_contains_banned_word: 'Your nickname contains bad word(s): %words%'  
  debug_mode: false  
  ads_custom_regex_pattern: ''  
  banned_words_custom_regex_pattern: ''  

***

# RU [EN](https://github.com/Qidan475/NoNickAds/#en-ru)
При входе на сервер игрока, плагин заменяет рекламу в его нике указанным текстом или просто банит его.

## Установка
Перекиньте файл NoNickAds.dll в папку Plugins.

## Пример конфига
NoNickAds:  
  is_enabled: true  
  whitelisted_players: \[76561198111120799@steam, 242306234269696000@discord, UnknownPole@northwood, notahubert@patreon]  
  ads: \[#yracribniulr, example]  
  use_smart_link_replacer: true  
  replacement_text: ''  
  lite_replacing: true  
  banned_words: \[nigga, nigger]  
  ban_duration_in_minutes: 120  
  use_unicode_normalization: false  
  unicode_normalization: FormC  
  kick_msg_when_nick_is_empty: Похоже, что ваш ник полностью состоит из рекламы. Измените ник и перезайдите на сервер.  
  ban_msg_when_nick_contains_banned_word: 'Ваш ник содержит запрещённое слово(слова): %words%'  
  debug_mode: false  
  ads_custom_regex_pattern: ''  
  banned_words_custom_regex_pattern: ''  
