---
name: ship
description: ビルド確認・ドキュメント更新・コミットを実行する
---

`msbuild OutlookArchiver/OutlookArchiver.vbproj` でビルドし、エラーがないことを確認した上で、必要に応じてドキュメント（CLAUDE.md・docs/ 以下）を更新し、適切な粒度でコミットを行ってください。

コミットの粒度の目安:
- 実装変更（ソースコード・.vbproj）と、それに伴うドキュメント更新は別コミットに分ける
- コミットメッセージは日本語で、変更の「なぜ」を説明する
- 作業に必要なコマンド(git add, git commit, git checkout, git diff, git log, git status, git show)は承認なく実行してください
