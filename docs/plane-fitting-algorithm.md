# Plane fitting: SVD least-squares + RANSAC

Algorithm notes for the math Norma ports to C# (`Norma.Core.Geometry`).
Source studied: [htcr/plane-fitting](https://github.com/htcr/plane-fitting) (Python,
NumPy), reviewed 2026-07-15. Sections 1–3 summarize that implementation; sections
4–5 are Norma's extensions.

## 1. Least-squares plane via SVD

The reference implementation (`fit_plane_LSE` in `fit_plane_LSE.py`) works in
**homogeneous coordinates**: each point is a row `[x, y, z, 1]` of an N×4 matrix
`A`, and a plane is the coefficient vector `p = [a, b, c, d]` of

```
a·x + b·y + c·z + d = 0
```

The least-squares plane minimizes `‖A·p‖` subject to `‖p‖ = 1`, whose solution is
the **right singular vector associated with the smallest singular value** of `A`
— i.e. after `U, S, Vt = svd(A)`, the plane is `Vt[-1, :]` (NumPy orders singular
values descending). Requires ≥ 3 points.

Point-to-plane distance (`get_point_dist`) for scoring:

```
dist_i = |A_i · p| / √(a² + b² + c²)
```

**C# port note.** An equivalent, numerically friendlier formulation (and the one
we implement in `SvdPlaneFitter`) subtracts the centroid `c̄` first and takes the
SVD of the N×3 centered matrix `M = A_xyz − c̄`. The unit normal `n̂` is the right
singular vector of the *smallest* singular value of `M`; the plane passes through
`c̄` (point–normal form, `PlaneFit.Normal` / `PlaneFit.Centroid`). This is
algebraically the same least-squares problem with the offset `d` eliminated, and
it avoids mixing metre-scale coordinates with the constant 1 column. Math.NET:
`Matrix<double>.Svd(computeVectors: true).VT.Row(2)`.

## 2. RANSAC outlier rejection

`fit_plane_LSE_RANSAC(points, iters=1000, inlier_thresh=0.05, return_outlier_list=False)`:

1. **Sample** — pick 3 points at random without replacement (the minimal set that
   determines a plane).
2. **Hypothesize** — fit a candidate plane to the 3 points with the SVD fit above.
3. **Score** — compute distances of *all* N points to the candidate; points with
   `dist < inlier_thresh` are inliers.
4. **Select** — keep the candidate with the highest inlier count across all
   `iters` iterations.
5. **Refit** — re-run the least-squares fit on the winning consensus set (all its
   inliers), then reclassify inliers against the refit plane with the same
   threshold. Report the fit variance of the final inlier distances.

Defaults in the reference: **1000 iterations**, inlier threshold **0.05** (in the
source data's units). Norma's defaults (`RansacOptions`): 1000 iterations,
**5 mm** threshold — tune per survey accuracy class. We also add an optional
`RandomSeed` so fits are reproducible in tests.

Notes for the port:

- Degenerate samples (3 collinear points) give an ill-conditioned fit; detect via
  a near-zero second singular value (or cross-product magnitude) and skip the
  iteration.
- The iteration count can be adapted from the expected outlier ratio
  (`k = log(1−p) / log(1−w³)`), but a fixed 1000 is cheap and fine at survey
  point-set sizes.

## 3. Fit quality

- **RMS orthogonal distance** of inliers to the final plane (`PlaneFit.RmsDistance`,
  mm) — the headline flatness/fit number.
- The reference reports variance of inlier distances; RMS is the same information
  in linear units.

## 4. Extension: angle between fitted and design normals

Given the fitted unit normal `n̂_fit` and the design plane's unit normal
`n̂_design`, the orientation deviation is

```
θ = arccos( |n̂_fit · n̂_design| )        θ ∈ [0°, 90°]
```

The absolute value makes the result sign-insensitive: a plane's normal direction
is defined only up to ±, so anti-parallel normals mean *zero* deviation. Clamp
the dot product to `[0, 1]` before `arccos` to guard against rounding just above
1.0. Implemented as `OrientationDeviation.NormalAngleDegrees`.

## 5. Extension: axis / centerline rotational misalignment

Best-fit **line** through a point set (e.g. points along a column or pipe
centerline) is the mirror image of the plane fit: center the points on their
centroid and take the right singular vector of the **largest** singular value —
the direction of maximum variance (`IAxisFitter`, `AxisFit`). RANSAC applies
unchanged with a 2-point minimal sample and point-to-line radial distance as the
inlier metric.

Rotational misalignment between the as-built axis `â_fit` and design axis
`â_design` uses the same sign-insensitive angle:

```
θ = arccos( |â_fit · â_design| )        θ ∈ [0°, 90°]
```

(`OrientationDeviation.AxisAngleDegrees`). Where a signed tilt *direction* is
needed later (which way is the column leaning?), the rotation axis is
`â_design × â_fit` — recorded here as a roadmap item, not in the first cut.

## Units

All lengths in Norma are **millimetres**; all reported angles are **degrees**.
CSV headers always state the unit.
