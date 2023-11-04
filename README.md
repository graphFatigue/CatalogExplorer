# CatalogExplorer

як працює?
1. у файлі ...\CatalogExplorer\CatalogExplorer\Extensions\ServiceExtensions.cs змінити рядок підключення до бд
2. застосувати міграції бд, щоб вона створилася разом з даними
3. запустити
приклад структури каталогів з операційної системи для імпорту:
D:\films
приклад контенту файлу для імпорту:
Creating Digital Images
Creating Digital Images\Resources
Creating Digital Images\Evidence
Creating Digital Images\Graphic Products
Creating Digital Images\Resources\Primary Sources
Creating Digital Images\Resources\Secondary Sources
Creating Digital Images\Graphic Products\Process
Creating Digital Images\Graphic Products\Final Product

структура каталогів, що вже існує в БД при цьому експортується в файл, який знаходиться в каталозі ...\CatalogExplorer\CatalogExplorer
для експорту в файл, потрібно просто придумати назву для файлу, він сам створиться
