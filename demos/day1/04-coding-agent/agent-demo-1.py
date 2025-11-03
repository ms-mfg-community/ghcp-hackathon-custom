"""
Day 1.4 Demo: GitHub Copilot Coding Agent - Multi-Step Problem Solving
Demonstrates how agents decompose and solve complex problems.

This code represents output that a Copilot Agent might generate
when asked: "Build a comprehensive logging and monitoring system with
multiple output channels, error tracking, and performance metrics."
"""

import time
import json
from datetime import datetime
from enum import Enum
from typing import Dict, List, Optional, Callable
from collections import deque
from dataclasses import dataclass, asdict


class LogLevel(Enum):
    """Log severity levels"""
    DEBUG = 1
    INFO = 2
    WARNING = 3
    ERROR = 4
    CRITICAL = 5


@dataclass
class LogEntry:
    """Represents a single log entry"""
    timestamp: str
    level: LogLevel
    message: str
    component: str
    duration_ms: Optional[float] = None
    error_details: Optional[str] = None
    metadata: Optional[Dict] = None


@dataclass
class PerformanceMetrics:
    """Performance tracking metrics"""
    component: str
    operation: str
    duration_ms: float
    memory_used_mb: float
    success: bool
    error_count: int = 0


# Step 1: Abstract output channel (Strategy Pattern)
class OutputChannel:
    """Base class for different output channels"""
    
    def write(self, entry: LogEntry) -> None:
        raise NotImplementedError
    
    def flush(self) -> None:
        pass


class ConsoleOutputChannel(OutputChannel):
    """Output logs to console"""
    
    def write(self, entry: LogEntry) -> None:
        level_color = {
            LogLevel.DEBUG: '\033[36m',    # Cyan
            LogLevel.INFO: '\033[32m',     # Green
            LogLevel.WARNING: '\033[33m',  # Yellow
            LogLevel.ERROR: '\033[31m',    # Red
            LogLevel.CRITICAL: '\033[1;31m' # Bold Red
        }
        reset_color = '\033[0m'
        
        color = level_color.get(entry.level, '')
        print(f"{color}[{entry.timestamp}] {entry.level.name:8} | "
              f"{entry.component:15} | {entry.message}{reset_color}")
        
        if entry.duration_ms:
            print(f"  Duration: {entry.duration_ms:.2f}ms")
        if entry.error_details:
            print(f"  Error: {entry.error_details}")


class FileOutputChannel(OutputChannel):
    """Output logs to file"""
    
    def __init__(self, filename: str):
        self.filename = filename
        self.buffer = []
    
    def write(self, entry: LogEntry) -> None:
        self.buffer.append(asdict(entry))
        if len(self.buffer) >= 10:
            self.flush()
    
    def flush(self) -> None:
        if not self.buffer:
            return
        
        with open(self.filename, 'a') as f:
            for entry in self.buffer:
                entry['level'] = entry['level'].name
                f.write(json.dumps(entry) + '\n')
        
        self.buffer.clear()


class MetricsOutputChannel(OutputChannel):
    """Output performance metrics"""
    
    def __init__(self):
        self.metrics: Dict[str, List[PerformanceMetrics]] = {}
    
    def write(self, entry: LogEntry) -> None:
        pass  # Metrics handled separately
    
    def add_metric(self, metric: PerformanceMetrics) -> None:
        if metric.component not in self.metrics:
            self.metrics[metric.component] = []
        self.metrics[metric.component].append(metric)
    
    def get_summary(self) -> Dict:
        """Generate metrics summary"""
        summary = {}
        for component, metrics in self.metrics.items():
            summary[component] = {
                'total_operations': len(metrics),
                'avg_duration_ms': sum(m.duration_ms for m in metrics) / len(metrics),
                'success_rate': sum(1 for m in metrics if m.success) / len(metrics) * 100,
                'total_errors': sum(m.error_count for m in metrics)
            }
        return summary


# Step 2: Error tracking system
class ErrorTracker:
    """Track and aggregate errors"""
    
    def __init__(self, max_history: int = 100):
        self.errors: deque = deque(maxlen=max_history)
        self.error_counts: Dict[str, int] = {}
    
    def record_error(self, error_type: str, message: str, component: str) -> None:
        """Record an error"""
        error_entry = {
            'timestamp': datetime.now().isoformat(),
            'type': error_type,
            'message': message,
            'component': component
        }
        self.errors.append(error_entry)
        self.error_counts[error_type] = self.error_counts.get(error_type, 0) + 1
    
    def get_error_summary(self) -> Dict:
        """Get error statistics"""
        return {
            'total_errors': sum(self.error_counts.values()),
            'error_types': self.error_counts,
            'recent_errors': list(self.errors)[-5:]
        }


# Step 3: Main logging and monitoring system
class LoggingAndMonitoringSystem:
    """
    Comprehensive logging and monitoring system.
    Demonstrates how an agent might build a complex, multi-component system.
    """
    
    def __init__(self):
        self.channels: List[OutputChannel] = []
        self.error_tracker = ErrorTracker()
        self.metrics_channel = MetricsOutputChannel()
        self.channels.append(self.metrics_channel)
    
    def add_channel(self, channel: OutputChannel) -> None:
        """Register an output channel"""
        self.channels.append(channel)
    
    def log(self, level: LogLevel, component: str, message: str,
            duration_ms: Optional[float] = None,
            error_details: Optional[str] = None,
            metadata: Optional[Dict] = None) -> None:
        """
        Log a message to all configured channels.
        """
        entry = LogEntry(
            timestamp=datetime.now().isoformat(),
            level=level,
            message=message,
            component=component,
            duration_ms=duration_ms,
            error_details=error_details,
            metadata=metadata
        )
        
        for channel in self.channels:
            channel.write(entry)
        
        # Track errors
        if level in (LogLevel.ERROR, LogLevel.CRITICAL):
            self.error_tracker.record_error(
                f"{level.name}",
                message,
                component
            )
    
    def track_operation(self, component: str, operation: str,
                       func: Callable, *args, **kwargs) -> any:
        """
        Track operation performance.
        Decorator-style performance monitoring.
        """
        start_time = time.time()
        
        try:
            result = func(*args, **kwargs)
            duration_ms = (time.time() - start_time) * 1000
            
            metric = PerformanceMetrics(
                component=component,
                operation=operation,
                duration_ms=duration_ms,
                memory_used_mb=0.5,  # Placeholder
                success=True
            )
            self.metrics_channel.add_metric(metric)
            
            self.log(LogLevel.INFO, component,
                    f"Operation '{operation}' completed successfully",
                    duration_ms=duration_ms)
            
            return result
        
        except Exception as e:
            duration_ms = (time.time() - start_time) * 1000
            
            metric = PerformanceMetrics(
                component=component,
                operation=operation,
                duration_ms=duration_ms,
                memory_used_mb=0.5,
                success=False,
                error_count=1
            )
            self.metrics_channel.add_metric(metric)
            
            self.log(LogLevel.ERROR, component,
                    f"Operation '{operation}' failed",
                    duration_ms=duration_ms,
                    error_details=str(e))
            
            self.error_tracker.record_error(
                type(e).__name__,
                str(e),
                component
            )
            
            raise
    
    def get_report(self) -> Dict:
        """Generate comprehensive report"""
        return {
            'timestamp': datetime.now().isoformat(),
            'metrics_summary': self.metrics_channel.get_summary(),
            'error_summary': self.error_tracker.get_error_summary()
        }
    
    def flush_all(self) -> None:
        """Flush all output channels"""
        for channel in self.channels:
            channel.flush()


# Step 4: Demonstration and usage
def simulate_application_workload(logger: LoggingAndMonitoringSystem):
    """Simulate typical application operations"""
    
    def process_data(data_size: int):
        """Simulate data processing"""
        time.sleep(0.1 * (data_size / 1000))
        return f"Processed {data_size} items"
    
    def validate_input(input_str: str):
        """Simulate input validation"""
        if not input_str:
            raise ValueError("Empty input")
        return len(input_str)
    
    # Successful operations
    logger.log(LogLevel.INFO, "Application", "Starting workload simulation")
    
    logger.track_operation("DataProcessor", "process_data_small",
                          process_data, 1000)
    
    logger.track_operation("DataProcessor", "process_data_large",
                          process_data, 5000)
    
    # Validation operations
    logger.log(LogLevel.DEBUG, "Validator", "Validating inputs")
    try:
        logger.track_operation("Validator", "validate_input",
                              validate_input, "test_data")
    except ValueError as e:
        pass  # Expected failures are logged
    
    # Error scenario
    try:
        logger.track_operation("Validator", "validate_input",
                              validate_input, "")
    except ValueError:
        pass
    
    logger.log(LogLevel.INFO, "Application", "Workload simulation complete")


# Main demonstration
if __name__ == "__main__":
    print("=" * 70)
    print("GitHub Copilot Coding Agent - Complex System Demonstration")
    print("=" * 70)
    print("\nThis system was generated by a Copilot Agent when asked to:")
    print("'Build a comprehensive logging and monitoring system'\n")
    
    # Create logging system
    logger = LoggingAndMonitoringSystem()
    logger.add_channel(ConsoleOutputChannel())
    logger.add_channel(FileOutputChannel("demo.log"))
    
    # Run simulation
    logger.log(LogLevel.INFO, "System", "Initializing logging and monitoring system")
    simulate_application_workload(logger)
    
    # Generate report
    logger.flush_all()
    
    print("\n" + "=" * 70)
    print("SYSTEM REPORT")
    print("=" * 70)
    
    report = logger.get_report()
    print(json.dumps(report, indent=2))
    
    print("\n" + "=" * 70)
    print("Demonstration Complete!")
    print("=" * 70)
