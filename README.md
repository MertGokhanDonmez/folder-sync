# folder-sync

One-way folder synchronization. The replica folder is kept as an exact copy of the source folder, repeated at a fixed interval.

## Usage

```
dotnet run -- <source> <replica> <interval> <log>
```

| Argument   | Description                                   |
|------------|-----------------------------------------------|
| `source`   | Folder to copy from (must exist)              |
| `replica`  | Folder to copy to (created if missing)        |
| `interval` | Seconds between synchronizations (positive)   |
| `log`      | Path of the log file                          |

Example:

```
dotnet run -- ./source ./replica 10 ./sync.log
```

Stop with `Ctrl + C`.

## Behavior

Each cycle:

1. Deletes files in the replica that no longer exist in the source.
2. Deletes directories in the replica that no longer exist in the source.
3. Copies new files, and files whose MD5 hash differs, from source to replica.

Every create, copy and delete operation is written to the console and to the log file with a timestamp.

## Known Limitations

Empty directories in the source are not copied to the replica, because directories are only created when a file is copied into them.
