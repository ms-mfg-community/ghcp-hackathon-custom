# Uncommitted Changes Detection

This directory contains utilities to detect uncommitted changes in the repository, helping prevent issues where build artifacts or untracked files cause false positives.

## Problem

The repository is a multi-language training environment (Python, .NET, Node.js/TypeScript, SQL) but the `.gitignore` file only contained .NET-specific patterns. This caused common build artifacts and development files to be detected as uncommitted changes when they should have been ignored.

## Solution

### 1. Enhanced .gitignore

Updated `.gitignore` to include patterns for all languages and tools used in this repository:

- **Python**: `__pycache__/`, `*.py[cod]`, `venv/`, `.pytest_cache/`
- **Node.js**: `node_modules/`, `package-lock.json`, build artifacts
- **IDE/Editors**: `.vscode/`, `.idea/`, `*.swp`
- **Environment**: `.env`, `.env.local`, `.env.test`
- **OS files**: `.DS_Store`, `Thumbs.db`
- **Playwright**: `test-results/`, `playwright-report/`
- **.NET**: (existing patterns preserved)

### 2. Validation Scripts

Two equivalent scripts for checking uncommitted changes:

#### Bash Script: `check-uncommitted-changes.sh`

```bash
./check-uncommitted-changes.sh
```

#### Python Script: `check_uncommitted_changes.py`

```bash
python3 check_uncommitted_changes.py
```

Both scripts:
- ✓ Detect modified/staged files
- ✓ Detect untracked files (excluding ignored files)
- ✓ Provide clear output with actionable suggestions
- ✓ Return appropriate exit codes (0 = clean, 1 = changes detected, 2 = error)

## Usage

### Check for Uncommitted Changes

Using Bash:
```bash
./check-uncommitted-changes.sh
```

Using Python:
```bash
python3 check_uncommitted_changes.py
```

### Exit Codes

- `0`: No uncommitted changes, working tree is clean
- `1`: Uncommitted changes or untracked files detected
- `2`: Error (e.g., not in a git repository)

### Example Output

When changes are detected:
```
❌ Uncommitted changes detected

Modified files:
M       .gitignore

To see detailed changes, run: git diff
To see staged changes, run: git diff --cached
```

When working tree is clean:
```
✓ No uncommitted changes detected
✓ Working tree is clean
```

## Integration

These scripts can be integrated into:
- Pre-commit hooks
- CI/CD pipelines
- Build validation
- Development workflows

### Pre-commit Hook Example

```bash
#!/bin/bash
# .git/hooks/pre-commit

./check-uncommitted-changes.sh
if [ $? -ne 0 ]; then
    echo "Cannot commit: uncommitted changes detected"
    exit 1
fi
```

### CI/CD Integration Example

```yaml
# .github/workflows/validate.yml
jobs:
  validate:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Check for uncommitted changes
        run: ./check-uncommitted-changes.sh
```

## Testing

To test the .gitignore patterns:

```bash
# Create test artifacts that should be ignored
mkdir -p __pycache__ node_modules venv .vscode
touch test.pyc .env
git status

# These files should NOT appear in git status
```

## Benefits

1. **Prevents False Positives**: Build artifacts are properly ignored
2. **Cross-platform**: Works on Windows, macOS, and Linux
3. **Multi-language Support**: Covers Python, Node.js, .NET, and more
4. **Clear Feedback**: Scripts provide actionable suggestions
5. **CI/CD Ready**: Easy to integrate into automation workflows

## Related Files

- `.gitignore` - Enhanced patterns for all languages
- `check-uncommitted-changes.sh` - Bash validation script
- `check_uncommitted_changes.py` - Python validation script
