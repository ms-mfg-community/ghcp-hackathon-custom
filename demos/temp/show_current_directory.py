"""
Script to display the current working directory and list repository contents.
"""
import os
from pathlib import Path

def show_current_directory():
    """Display the current working directory."""
    current_dir = os.getcwd()
    print(f"Current directory: {current_dir}")
    return current_dir

def list_repository_contents(repo_root=None):
    """
    List all files and directories in the repository.
    
    Args:
        repo_root: Path to repository root. If None, uses current directory.
    """
    if repo_root is None:
        repo_root = os.getcwd()
    
    repo_path = Path(repo_root)
    
    print(f"\n{'='*80}")
    print(f"Repository Contents: {repo_path}")
    print(f"{'='*80}\n")
    
    # Separate files and directories
    directories = []
    files = []
    
    for item in sorted(repo_path.rglob('*')):
        # Skip hidden directories and common exclusions
        if any(part.startswith('.') for part in item.parts):
            continue
        if any(excluded in item.parts for excluded in ['__pycache__', 'node_modules', 'bin', 'obj']):
            continue
            
        relative_path = item.relative_to(repo_path)
        
        if item.is_dir():
            directories.append(relative_path)
        else:
            files.append(relative_path)
    
    # Print directories
    print(f"Directories ({len(directories)}):")
    print("-" * 80)
    for directory in directories[:50]:  # Limit output
        print(f"  📁 {directory}")
    if len(directories) > 50:
        print(f"  ... and {len(directories) - 50} more directories")
    
    # Print files
    print(f"\nFiles ({len(files)}):")
    print("-" * 80)
    for file in files[:50]:  # Limit output
        print(f"  📄 {file}")
    if len(files) > 50:
        print(f"  ... and {len(files) - 50} more files")
    
    print(f"\nTotal: {len(directories)} directories, {len(files)} files")

if __name__ == "__main__":
    current = show_current_directory()
    
    # Navigate to repository root (assuming we're somewhere in ghcp-hackathon-custom)
    repo_root = Path(current)
    while repo_root.name != 'ghcp-hackathon-custom' and repo_root.parent != repo_root:
        repo_root = repo_root.parent
    
    if repo_root.name == 'ghcp-hackathon-custom':
        # List only the day1 folder
        day1_path = repo_root / 'demos' / 'day1'
        if day1_path.exists():
            list_repository_contents(day1_path)
        else:
            print(f"\nday1 folder not found at: {day1_path}")
    else:
        print("\nCould not find repository root. Listing current directory instead:")
        list_repository_contents(current)
