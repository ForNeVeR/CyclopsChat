CyclopsChat Maintainership
==========================

Release
-------

To release a new version:
1. Update the copyright year in [the license file][license], if required.
2. Choose the new version according to [Semantic Versioning][semver]. It should consist of three numbers (i.e. `1.0.0`).
3. Change the version number in `Cyclops.Core/Properties/AssemblyInfo.Shared.cs` (2 occurrences).
4. Make sure there's a properly formed version entry in [the changelog][changelog].
5. If there were any dependency updates, then ensure that the third-party licenses are up-to-date (see `THIR-PARTY-SOFTWARE.md` and the `third-party` directory contents).
6. Push a tag named `v<VERSION>` to GitHub.

The new release will be published automatically.

[changelog]: ./CHANGELOG.md
[license]: ./LICENSE.md
[semver]: https://semver.org/spec/v2.0.0.html
