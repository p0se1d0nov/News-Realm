# Define your item pipelines here
#
# Don't forget to add your pipeline to the ITEM_PIPELINES setting
# See: https://docs.scrapy.org/en/latest/topics/item-pipeline.html


# useful for handling different item types with a single interface
from itemadapter import ItemAdapter
import psycopg2
from psycopg2 import sql
from datetime import datetime


class TheNewsPipeline:
    @classmethod
    def from_crawler(cls, crawler):
        return cls(
            host=crawler.settings.get("PG_HOST", "localhost"),
            port=crawler.settings.getint("PG_PORT", 5432),
            user=crawler.settings.get("PG_USER", "news_database_admin"),
            password=crawler.settings.get("PG_PASSWORD", "1234567890"),
            database=crawler.settings.get("PG_DATABASE", "the_news")
        )

    def __init__(self, host, port, user, password, database):
        self.pg_host = host
        self.pg_port = port
        self.pg_user = user
        self.pg_password = password
        self.pg_database = database
        self.conn = None
        self.cur = None

    def open_spider(self, spider):
        self.conn = psycopg2.connect(
            host=self.pg_host,
            port=self.pg_port,
            user=self.pg_user,
            password=self.pg_password,
            dbname=self.pg_database
        )
        self.cur = self.conn.cursor()
        # Создаём таблицу news если её нет с улучшенной схемой
        self.cur.execute("""
        CREATE TABLE IF NOT EXISTS news (
            id SERIAL PRIMARY KEY,
            source VARCHAR(50) NOT NULL DEFAULT 'rss',
            title TEXT NOT NULL,
            description TEXT,
            maintext TEXT,
            image_url TEXT,
            authors VARCHAR(500),
            category VARCHAR(100),
            date_publish DATE,
            time_publish TIME,
            language VARCHAR(10) NOT NULL DEFAULT 'ru',
            url TEXT NOT NULL UNIQUE,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        )
        """)
        # Создаём индексы для оптимизации запросов
        indexes = [
            "CREATE INDEX IF NOT EXISTS idx_news_category ON news(category)",
            "CREATE INDEX IF NOT EXISTS idx_news_date_publish ON news(date_publish)",
            "CREATE INDEX IF NOT EXISTS idx_news_language ON news(language)",
            "CREATE INDEX IF NOT EXISTS idx_news_source ON news(source)",
            "CREATE INDEX IF NOT EXISTS idx_news_created_at ON news(created_at)",
            "CREATE INDEX IF NOT EXISTS idx_news_url ON news(url)"
        ]
        for index_sql in indexes:
            try:
                self.cur.execute(index_sql)
            except psycopg2.Error:
                pass  # Индекс уже существует или ошибка создания
        self.conn.commit()

    def close_spider(self, spider):
        if self.cur:
            self.cur.close()
        if self.conn:
            self.conn.close()

    def parse_date(self, date_str):
        """Преобразует строку даты формата DD.MM.YYYY в объект date"""
        if not date_str:
            return None
        try:
            return datetime.strptime(date_str, "%d.%m.%Y").date()
        except (ValueError, TypeError):
            return None

    def parse_time(self, time_str):
        """Преобразует строку времени формата HH:MM:SS в объект time"""
        if not time_str:
            return None
        try:
            return datetime.strptime(time_str, "%H:%M:%S").time()
        except (ValueError, TypeError):
            return None

    def process_item(self, item, spider):
        # Поддерживает dict или scrapy.Item
        data = dict(item)
        
        # Преобразуем дату и время в правильные типы
        date_publish = self.parse_date(data.get("date_publish"))
        time_publish = self.parse_time(data.get("time_publish"))
        
        values = (
            data.get("source") or "rss",
            data.get("title") or "",
            data.get("description"),
            data.get("maintext"),
            data.get("image_url"),
            data.get("authors"),
            data.get("category"),
            date_publish,
            time_publish,
            data.get("language") or "ru",
            data.get("url") or "",
        )
        insert_sql = """
        INSERT INTO news (
            source, title, description, maintext, image_url,
            authors, category, date_publish, time_publish, language, url
        ) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
        ON CONFLICT (url) DO NOTHING;
        """
        try:
            self.cur.execute(insert_sql, values)
            self.conn.commit()
        except psycopg2.IntegrityError:
            # Игнорируем дубликаты
            self.conn.rollback()
        except Exception as e:
            spider.logger.error(f"Ошибка при вставке данных: {e}")
            self.conn.rollback()
        return item