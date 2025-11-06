import sqlite3
import os

# Database path
db_path = 'calculator_tests.db'

if not os.path.exists(db_path):
    print(f"Database not found: {db_path}")
    exit(1)

# Connect to database
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# Get all tables
cursor.execute("SELECT name FROM sqlite_master WHERE type='table'")
tables = cursor.fetchall()
print(f"Available tables: {[t[0] for t in tables]}\n")

# Try to find the correct table name
table_name = None
for table in tables:
    if 'test' in table[0].lower() or 'calculator' in table[0].lower():
        table_name = table[0]
        break

if not table_name and tables:
    table_name = tables[0][0]

if table_name:
    print(f"Using table: {table_name}\n")
    print("=" * 80)
    print("ALL TEST CASES IN DATABASE")
    print("=" * 80)
    
    # Query all test cases
    cursor.execute(f"SELECT * FROM {table_name} ORDER BY Id")
    rows = cursor.fetchall()
    
    # Get column names
    cursor.execute(f"PRAGMA table_info({table_name})")
    columns = [col[1] for col in cursor.fetchall()]
    
    print(f"\nTotal test cases: {len(rows)}\n")
    
    for row in rows:
        print(f"{'─' * 80}")
        for i, col in enumerate(columns):
            if i < len(row):
                print(f"{col:20s}: {row[i]}")
        print()
else:
    print("No tables found in database")

conn.close()
