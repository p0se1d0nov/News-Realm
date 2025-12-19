#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Скрипт для запуска всех пауков Scrapy в проекте
Можно вызывать из C# программы
"""

import sys
import os
from scrapy.crawler import CrawlerProcess
from scrapy.utils.project import get_project_settings

def run_all_spiders():
    """Запускает все пауки в проекте"""
    # Получаем настройки проекта
    settings = get_project_settings()
    
    # Создаем процесс краулера
    process = CrawlerProcess(settings)
    
    # Получаем список всех пауков из проекта
    from scrapy.utils.spider import iter_spider_classes
    from scrapy.spiderloader import SpiderLoader
    
    spider_loader = SpiderLoader.from_settings(settings)
    spider_names = spider_loader.list()
    
    if not spider_names:
        print("Не найдено ни одного паука в проекте!")
        return
    
    print(f"Найдено пауков: {len(spider_names)}")
    for spider_name in spider_names:
        print(f"  - {spider_name}")
        process.crawl(spider_name)
    
    print("\nЗапуск всех пауков...")
    process.start()  # Блокирующий вызов - запускает все пауки
    print("Все пауки завершили работу.")

if __name__ == "__main__":
    # Убеждаемся, что мы в правильной директории
    script_dir = os.path.dirname(os.path.abspath(__file__))
    os.chdir(script_dir)
    
    run_all_spiders()

