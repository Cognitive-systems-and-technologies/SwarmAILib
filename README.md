# SwarmAILib

C#-библиотека для реализации роевых алгоритмов

Включает в себя следующие файлы:

## ACO.cs 
Реализует алгоритм муравьиной колонии.

Включает в себя класс Ant для реализации собственно "муравья" и класс AntColonyAlgorithm для реализации алгоритма муравьиной колонии.


## BCA.cs

Реализует алгоритм пчелиной колонии. Включает в себя класс Bee для отдельной "пчелы" и класс BeeColonyAlgorithm для реализации алгоритма пчелиной колонии.

## PSO.cs 

Реализует алгоритм роя частиц. Включает в себя класс Particle для отдельной частицы и класс ParticleSwarmOptimization для реализации алгоритма пчелиной колонии.


## Maze.cs
Включает в себя класс Maze, реализующий лабиринт.

## Установка библиотеки

Скачиваем репозиторий (с помощью git clone или вручную). 

Далее библиотеку необходимо скомпилировать. 

Использование библиотеки не зависит от операционной системы, но зависит от применяемой Вами среды разработки. 

Для работы с проектами на C# в Windows рекомендуется Visual Studio (https://visualstudio.microsoft.com/ru/), для работы в Linux или MacOS - среда MonoDevelop (https://www.monodevelop.com/). Существует версия Monodevelop для Windows и версия Visual Studio для MacOS.

### Работа в Visual Studio

Открываем в скачанном репозитории файл SwarmAILib.csproj.

В главном меню выбираем "Сборка", затем "Собрать решение".

Создаём новый проект в Visual Studio.

В "Обозревателе решений" правой кнопкой кликаем на пункт "Зависимости", выбираем "Добавить ссылку на проект".

Нажимаем "Обзор" и выбираем файл SwarmAILib/bin/Debug/.net6.0/SwarmAILib.dll.

После этого мы можем использовать функции подключённых файлов в наших проектах.

### Работа в MonoDevelop
Создаём новый проект в среде MonoDevelop.
Затем на панели "Solution" кликаем по названию проекта, выбираем "Add" и добавляем все нужные классы из проекта (находятся в папке SwarmAILIb).
После этого используем как обычные классы в проекте.




