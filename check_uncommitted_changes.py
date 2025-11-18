#!/usr/bin/env python3
"""
check_uncommitted_changes.py
Script to detect uncommitted changes in the repository.
Returns exit code 0 if no uncommitted changes, 1 if changes detected.
"""

import subprocess
import sys
from pathlib import Path


def run_command(cmd):
    """Run a git command and return the output."""
    try:
        result = subprocess.run(
            cmd,
            capture_output=True,
            text=True,
            check=True
        )
        return result.stdout.strip(), result.returncode
    except subprocess.CalledProcessError as e:
        return e.stdout.strip(), e.returncode


def is_git_repository():
    """Check if the current directory is a git repository."""
    _, returncode = run_command(['git', 'rev-parse', '--git-dir'])
    return returncode == 0


def check_uncommitted_changes():
    """Check for uncommitted changes in the repository."""
    # Check if we're in a git repository
    if not is_git_repository():
        print("Error: Not in a git repository")
        return 2

    # Update git index to ensure we're checking the latest state
    run_command(['git', 'update-index', '--refresh'])

    # Check for uncommitted changes (both staged and unstaged)
    _, returncode = run_command(['git', 'diff-index', '--quiet', 'HEAD', '--'])
    
    if returncode != 0:
        print("❌ Uncommitted changes detected")
        print()
        print("Modified files:")
        output, _ = run_command(['git', 'diff-index', '--name-status', 'HEAD', '--'])
        print(output)
        print()
        print("To see detailed changes, run: git diff")
        print("To see staged changes, run: git diff --cached")
        return 1

    # Check for untracked files (excluding ignored files)
    output, _ = run_command(['git', 'ls-files', '--others', '--exclude-standard'])
    
    if output:
        print("❌ Untracked files detected")
        print()
        print("Untracked files:")
        print(output)
        print()
        print("To add these files, run: git add <file>")
        print("To ignore these files, add them to .gitignore")
        return 1

    print("✓ No uncommitted changes detected")
    print("✓ Working tree is clean")
    return 0


if __name__ == '__main__':
    exit_code = check_uncommitted_changes()
    sys.exit(exit_code)
