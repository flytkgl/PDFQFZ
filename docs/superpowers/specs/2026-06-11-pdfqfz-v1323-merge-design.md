# PDFQFZ v1.32.3 Merge Design

## Goal

Use `D:\Downloads\PDFQFZ-v1.32.3` as the functional baseline, absorb its source and project-configuration changes into the current workspace, and preserve the local fix that prevents UI hangs when previewing large PDFs.

## Scope

- Merge source-level changes from `v1.32.3`, especially `Form1.cs`, `Form1.Designer.cs`, `Library\IniFileHelper.cs`, and `PDFQFZ.csproj`.
- Ignore build output and local-environment noise such as `.vs`, `bin`, `obj`, and `.csproj.user`.
- Reapply the local preview optimization so PDF preview loads pages lazily instead of rendering the entire document on selection.
- Keep the page-cache regression tests in the workspace and make sure the project still builds.

## Approach

1. Diff the current workspace against `v1.32.3` and isolate real source/config changes from generated artifacts.
2. Replace or merge the source files to match `v1.32.3` behavior where possible.
3. Reintegrate the lazy preview/page-cache fix into the merged `Form1` codepath.
4. Build the WinForms solution and run the targeted cache tests.

## Risks

- `Form1.cs` is a large conflict surface and may have overlapping logic changes in the preview workflow.
- `v1.32.3` project-file changes may alter references or packaging behavior and need validation after merge.
- Designer and event wiring may diverge from the current preview assumptions and must be checked after code merge.
