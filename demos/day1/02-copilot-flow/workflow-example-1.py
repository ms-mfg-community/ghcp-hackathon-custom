"""
Day 1.2 Demo: GitHub Copilot Flow - Multi-Turn Conversation Pattern
Demonstrates iterative refinement of code through Copilot conversations.

Simulated Conversation:
Turn 1: "Create a data processing pipeline that reads data, transforms it, and outputs results"
Turn 2: "Add error handling and logging"
Turn 3: "Include validation before processing"
Turn 4: "Optimize for performance with batch processing"
Turn 5: "Add progress tracking"
"""

import json
import logging
from typing import List, Dict, Optional
from dataclasses import dataclass, asdict
from datetime import datetime
import time

# Configure logging - Added in Turn 2
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)


@dataclass
class DataRecord:
    """Represents a single data record"""
    id: int
    name: str
    value: float
    timestamp: str = None
    
    def __post_init__(self):
        if self.timestamp is None:
            self.timestamp = datetime.now().isoformat()


class DataValidator:
    """Turn 3: Added validation before processing"""
    
    @staticmethod
    def validate_record(record: Dict) -> tuple[bool, str]:
        """
        Validate a single record before processing.
        Returns (is_valid, error_message)
        """
        if not record.get('id'):
            return False, "Missing required field: id"
        if not record.get('name'):
            return False, "Missing required field: name"
        if 'value' not in record:
            return False, "Missing required field: value"
        
        try:
            float(record['value'])
        except (ValueError, TypeError):
            return False, f"Invalid value type: {record['value']}"
        
        return True, ""
    
    @staticmethod
    def validate_batch(records: List[Dict]) -> tuple[List[Dict], List[Dict]]:
        """
        Validate batch of records.
        Returns (valid_records, invalid_records)
        """
        valid = []
        invalid = []
        
        for record in records:
            is_valid, error = DataValidator.validate_record(record)
            if is_valid:
                valid.append(record)
            else:
                invalid.append({'record': record, 'error': error})
        
        return valid, invalid


class DataTransformer:
    """Transforms raw data into processed format"""
    
    @staticmethod
    def transform_record(record: Dict) -> Optional[DataRecord]:
        """
        Transform raw dictionary into DataRecord.
        Turn 2: Added error handling
        """
        try:
            return DataRecord(
                id=int(record['id']),
                name=str(record['name']).strip().upper(),
                value=float(record['value']) * 1.1  # 10% increase as transformation
            )
        except (KeyError, ValueError, TypeError) as e:
            logger.warning(f"Failed to transform record: {record}. Error: {e}")
            return None
    
    @staticmethod
    def filter_by_value(records: List[DataRecord], min_value: float) -> List[DataRecord]:
        """Filter records by minimum value threshold"""
        return [r for r in records if r.value >= min_value]
    
    @staticmethod
    def aggregate_by_name(records: List[DataRecord]) -> Dict[str, Dict]:
        """Aggregate records by name"""
        aggregated = {}
        for record in records:
            if record.name not in aggregated:
                aggregated[record.name] = {
                    'count': 0,
                    'total': 0,
                    'average': 0,
                    'max': float('-inf'),
                    'min': float('inf')
                }
            
            agg = aggregated[record.name]
            agg['count'] += 1
            agg['total'] += record.value
            agg['average'] = agg['total'] / agg['count']
            agg['max'] = max(agg['max'], record.value)
            agg['min'] = min(agg['min'], record.value)
        
        return aggregated


class ProgressTracker:
    """Turn 5: Added progress tracking"""
    
    def __init__(self, total: int, name: str = "Processing"):
        self.total = total
        self.current = 0
        self.name = name
        self.start_time = time.time()
    
    def update(self, amount: int = 1):
        """Update progress"""
        self.current += amount
        self._print_progress()
    
    def _print_progress(self):
        """Print progress bar"""
        percent = (self.current / self.total) * 100
        filled = int(20 * self.current / self.total)
        bar = '█' * filled + '░' * (20 - filled)
        
        elapsed = time.time() - self.start_time
        if self.current > 0:
            rate = self.current / elapsed
            remaining = (self.total - self.current) / rate if rate > 0 else 0
        else:
            remaining = 0
        
        logger.info(f"{self.name} [{bar}] {percent:.1f}% ({self.current}/{self.total}) "
                   f"Elapsed: {elapsed:.1f}s Remaining: {remaining:.1f}s")


class DataPipeline:
    """
    Turn 1-5: Complete data processing pipeline with iterative improvements
    - Reads raw data
    - Validates records
    - Transforms data
    - Processes in batches for performance (Turn 4)
    - Tracks progress (Turn 5)
    """
    
    def __init__(self, batch_size: int = 10):
        self.batch_size = batch_size
        self.total_processed = 0
        self.total_errors = 0
    
    def read_data(self, data: List[Dict]) -> List[Dict]:
        """Read and validate input data"""
        logger.info(f"Reading {len(data)} records")
        return data
    
    def process(self, raw_data: List[Dict]) -> Dict:
        """
        Turn 4: Optimized batch processing for performance
        Process raw data through validation, transformation, and aggregation.
        """
        logger.info("=== Starting Data Pipeline ===")
        
        # Step 1: Validate
        logger.info("Step 1: Validating data...")
        valid_records, invalid_records = DataValidator.validate_batch(raw_data)
        
        if invalid_records:
            logger.warning(f"Found {len(invalid_records)} invalid records")
            for item in invalid_records:
                logger.warning(f"  Record {item['record'].get('id')}: {item['error']}")
        
        self.total_errors = len(invalid_records)
        
        # Step 2: Transform with batch processing and progress tracking
        logger.info("Step 2: Transforming data...")
        tracker = ProgressTracker(len(valid_records), "Transforming")
        
        transformed_records = []
        for i, record in enumerate(valid_records):
            if i % self.batch_size == 0 and i > 0:
                logger.info(f"  Batch processed: {i} records")
            
            transformed = DataTransformer.transform_record(record)
            if transformed:
                transformed_records.append(transformed)
            
            tracker.update()
        
        # Step 3: Filter and aggregate
        logger.info("Step 3: Filtering and aggregating...")
        filtered = DataTransformer.filter_by_value(transformed_records, 100)
        aggregated = DataTransformer.aggregate_by_name(filtered)
        
        self.total_processed = len(transformed_records)
        
        logger.info("=== Pipeline Complete ===")
        
        return {
            'input_count': len(raw_data),
            'valid_count': len(valid_records),
            'invalid_count': len(invalid_records),
            'transformed_count': len(transformed_records),
            'filtered_count': len(filtered),
            'aggregated': aggregated,
            'records': [asdict(r) for r in filtered]
        }


# Demonstration
if __name__ == "__main__":
    print("=" * 70)
    print("GitHub Copilot Flow - Multi-Turn Conversation Demonstration")
    print("=" * 70)
    
    # Generate sample data
    raw_data = [
        {'id': 1, 'name': 'product_a', 'value': 100},
        {'id': 2, 'name': 'product_b', 'value': 150},
        {'id': 3, 'name': 'product_a', 'value': 120},
        {'id': 4, 'name': 'product_c', 'value': 200},
        {'id': 5, 'name': 'product_b', 'value': 180},
        {'id': 6, 'name': '', 'value': 90},  # Invalid: missing name
        {'id': 7, 'name': 'product_c', 'value': 'invalid'},  # Invalid: bad value
        {'id': 8, 'name': 'product_a', 'value': 95},
        {'id': 9, 'name': 'product_d', 'value': 210},
        {'id': 10, 'name': 'product_b', 'value': 175},
    ]
    
    # Run pipeline
    pipeline = DataPipeline(batch_size=3)
    result = pipeline.process(raw_data)
    
    # Display results
    print("\n" + "=" * 70)
    print("PIPELINE RESULTS")
    print("=" * 70)
    print(f"Input Records:       {result['input_count']}")
    print(f"Valid Records:       {result['valid_count']}")
    print(f"Invalid Records:     {result['invalid_count']}")
    print(f"Transformed:         {result['transformed_count']}")
    print(f"After Filtering:     {result['filtered_count']}")
    
    print("\nAggregated Summary:")
    for name, agg in result['aggregated'].items():
        print(f"\n  {name}:")
        print(f"    Count:   {agg['count']}")
        print(f"    Total:   {agg['total']:.2f}")
        print(f"    Average: {agg['average']:.2f}")
        print(f"    Min:     {agg['min']:.2f}")
        print(f"    Max:     {agg['max']:.2f}")
    
    print("\n" + "=" * 70)
    print("Demonstration Complete!")
    print("=" * 70)
