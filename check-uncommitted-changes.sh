#!/bin/bash
# check-uncommitted-changes.sh
# Script to detect uncommitted changes in the repository
# Returns exit code 0 if no uncommitted changes, 1 if changes detected

set -e

# Check if we're in a git repository
if ! git rev-parse --git-dir > /dev/null 2>&1; then
    echo "Error: Not in a git repository"
    exit 2
fi

# Update git index to ensure we're checking the latest state
git update-index --refresh > /dev/null 2>&1 || true

# Check for uncommitted changes (both staged and unstaged)
if ! git diff-index --quiet HEAD --; then
    echo "❌ Uncommitted changes detected"
    echo ""
    echo "Modified files:"
    git diff-index --name-status HEAD --
    echo ""
    echo "To see detailed changes, run: git diff"
    echo "To see staged changes, run: git diff --cached"
    exit 1
fi

# Check for untracked files (excluding ignored files)
UNTRACKED=$(git ls-files --others --exclude-standard)
if [ -n "$UNTRACKED" ]; then
    echo "❌ Untracked files detected"
    echo ""
    echo "Untracked files:"
    echo "$UNTRACKED"
    echo ""
    echo "To add these files, run: git add <file>"
    echo "To ignore these files, add them to .gitignore"
    exit 1
fi

echo "✓ No uncommitted changes detected"
echo "✓ Working tree is clean"
exit 0
