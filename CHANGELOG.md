Changelog
=========

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Fixed
- [#50: Crash when opening a hyperlink](https://github.com/ForNeVeR/CyclopsChat/issues/50))
- Cyrillic hyperlinks aren't highlighted

## [2.0.1] - 2022-05-07
### Fixed
- Changing the bookmarks in any way no longer clears the bookmark storage
- Auto-reconnection now works again
- Invalid bookmarks received from server won't trigger reconnect now

## [2.0.0] - 2022-01-05
### Removed
- Special support for vk.com server
- Registration support ([see #32 for details](https://github.com/ForNeVeR/CyclopsChat/issues/32))

### Changed
- Changed the way we store the saved room password, in a backwards-incompatible way
- Update to .NET 6
- Use SharpXMPP instead of Jabber-Net

### Added
- Verbose protocol logging

## [1.0.9] - 2011-09-18

_This version was marked as "CyclopsChat 1.0.9 RC2, Stable" on the original CodePlex site._

### Fixed
- Bug with empty messages (composing notifications in conference)
- Bug with context menu on roster and tabs
- Some localization errors

### Added
- Nickname conflict dialog
- New tooltips
- Role and affiliation view on user list
- A splash screen

## [1.0.8-rc1] - 2011-08-14

_This version was marked as "CyclopsChat 1.0.8 RC1, Beta" on the original CodePlex site._

### Added
- Bookmarks support
- User highlighting when there are messages addressing them
- Added context menu for tabs, output and input areas
- **Disable all sounds** button

### Changed
- Login screen redesign

## [1.0.7] - 2011-05-19
### Added
- Error popup notifications
- Chat log support
- Program settings dialog

## [1.0.6] - 2011-05-11
### Added
- Room password support
- vk.com server support
- Ability to create a new room

## [1.0.5] - 2011-05-07
### Fixed
- UI rendering problems

### Added
- VCard editing

## [1.0.4-beta4] - 2011-05-02

_This version was marked as "CyclopsChat Beta 4" on the original CodePlex site._

### Fixed
- Reconnect on the login view
- jabber.org server workaround

### Added
- Full project localization (Russian, English)
- Room subject dialog

## [1.0.0-beta3] - 2011-04-27

_This version was marked as "Cyclops Chat Beta 3" on the original CodePlex site._

## Added
- Ability to register on server
- Tab notifications
- Nickname change handling

## [1.0.0-beta2] - 2011-04-22

_This version was marked as "Cyclops Chat Beta 2" on the original CodePlex site._

### Added
- Error indicator on the login screen
- Debug console

## [1.0.0-beta1_1] - 2011-04-20

_This version was marked as "Cyclops Chat Beta 1" on the original CodePlex site. Yes, "1.0 Beta 1" and "Cyclops Chat Beta 1" were existing simultaneously, thus the `-beta1_1` marking._

### Added
- User-defined avatar support

### Changed
- Digest-MD5 authentication is now used

## [1.0.0-beta1] - 2011-04-18

_This version was marked as "1.0 Beta 1" on the original CodePlex site._

Initial publicly available release, which supported the following features:

- Multi-user chat support (user can close/create/join to conferences)
- Privates (double-click on user to start one)
- Animated smiles (JISP-compatible)
- Handles all sorts of XMPP events: kicks, bans, nickname conflicts, CAPTCHA requests, disconnects
- Taskbar notification area indicator

[1.0.0-beta1]: https://github.com/ForNeVeR/CyclopsChat/releases/tag/v1.0.0-beta1
[1.0.0-beta1_1]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.0-beta1...v1.0.0-beta1_1
[1.0.0-beta2]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.0-beta1_1...v1.0.0-beta2
[1.0.0-beta3]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.0-beta2...v1.0.0-beta3
[1.0.4-beta4]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.0-beta3...v1.0.4-beta4
[1.0.5]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.4-beta4...v1.0.5
[1.0.6]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.5...v1.0.6
[1.0.7]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.6...v1.0.7
[1.0.8-rc1]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.7...v1.0.8-rc1
[1.0.9]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.8-rc1...v1.0.9
[2.0.0]: https://github.com/ForNeVeR/CyclopsChat/compare/v1.0.9...v2.0.0
[2.0.1]: https://github.com/ForNeVeR/CyclopsChat/compare/v2.0.0...v2.0.1
[Unreleased]: https://github.com/ForNeVeR/CyclopsChat/compare/v2.0.1...HEAD
