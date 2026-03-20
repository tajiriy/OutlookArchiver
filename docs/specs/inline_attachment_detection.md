# インライン添付ファイルの判定仕様

## 概要

メールの添付ファイルには「通常の添付」と「インライン画像（HTML本文中に埋め込まれた画像）」がある。
OutlookVault ではインライン画像を添付パネルに表示せず、HTML本文内で表示する。

## 判定ルール

| ContentId | HTML本文に cid:参照 | 画像ファイル | 判定 |
|-----------|-------------------|-----------|------|
| なし | - | - | 添付ファイル |
| あり | あり（完全一致） | - | インライン |
| あり | なし | はい (.png, .jpg 等) | インライン（推定） |
| あり | なし | いいえ (.pdf, .docx 等) | 添付ファイル |

## cid 参照の完全一致判定

HTML本文中の `cid:xxx` を検索する際、部分一致を防止する。
例: `cid:wm-qrcode` が `cid:wm-qrcode-mo` にマッチしないよう、
ContentId の直後の文字が英数字・ハイフン・アンダースコア以外であることを確認する。

## 既知の制約

### ContentId 付き画像ファイルの誤判定

Gmail などのメールクライアントは、通常の添付ファイル（非インライン）にも ContentId を付与する。
このため、以下のケースで誤判定が発生する。

**ケース: Gmail から画像ファイルを通常添付として送信**
- ContentId あり、画像ファイル、HTML本文に cid 参照なし
- OutlookVault の判定: インライン（推定） → 添付パネルに表示されない
- 期待される判定: 通常の添付ファイル → 添付パネルに表示される

### 誤判定が発生する理由

MIME 規格では `Content-Disposition: inline` と `Content-Disposition: attachment` で区別するが、
Outlook COM API（PropertyAccessor）ではこのヘッダーを直接取得できない。
利用可能な情報は ContentId（`PR_ATTACH_CONTENT_ID`）のみであり、
ContentId の有無だけではインラインか通常添付かを確実に判定できない。

### 現在のルールを採用した理由

「ContentId あり + 画像 = インライン」と推定することで、以下のメリットがある。
- Outlook ウェルカムメール等のレスポンシブメールで、装飾用画像が添付パネルに大量表示されるのを防止
- HTML本文中で cid 参照されていないが、CSS やメディアクエリで使用される画像を正しく非表示にできる

デメリットとして、Gmail から画像ファイルを通常添付した場合に添付パネルに表示されない。
ただし、ファイル自体は DB およびファイルシステムに保存されているため、データの損失はない。

### 一般的なメーラーでの同様の問題

この問題は OutlookVault 固有ではなく、一般的なメーラーでも発生する。
- Thunderbird: Content-Disposition ヘッダーで判定するが、Gmail が通常添付に inline を付けるケースで誤判定
- Outlook: 内部的に同じ問題を持ち、Gmail 発メールで添付アイコン表示が不安定になることがある

## 関連ファイル

- `OutlookService.vb` の `SaveAttachments` メソッド: インライン判定ロジック
- `OutlookService.vb` の `IsCidReferenced` メソッド: cid 完全一致判定
- `OutlookService.vb` の `IsImageExtension` メソッド: 画像拡張子判定
- `EmailPreviewControl.vb` の `LoadAttachments` メソッド: IsInline=True のファイルをスキップ
- `ImportService.vb` の `ProcessMailItem` メソッド: has_attachments フラグの修正
