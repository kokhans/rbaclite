// MIT License
//
// Copyright (c) 2022 Serhii Kokhan
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using RbacLite.Core.Models.Abstracts;

namespace RbacLite.Core.Providers.Abstracts;

public interface IRbacProvider<TRole, TPermission, TRolePermission>
where TRole : IRole
where TPermission : IPermission
where TRolePermission : IRolePermission
{
    Task<TRole> CreateRoleAsync(
        string systemName,
        string displayName,
        string? description,
        CancellationToken cancellationToken = default
    );

    Task<TRole> UpdateRoleAsync(
        TRole role,
        CancellationToken cancellationToken = default
    );

    Task DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task DeleteRoleAsync(
        TRole role,
        CancellationToken cancellationToken = default
    );

    Task<TRole> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public Task<bool> ExistsRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<TPermission> CreatePermissionAsync(
        string systemName,
        string displayName,
        string? description = default,
        CancellationToken cancellationToken = default
    );

    Task<TPermission> UpdatePermissionAsync(
        TPermission permission,
        CancellationToken cancellationToken = default
    );

    Task DeletePermissionAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task DeletePermissionAsync(
        TPermission permission,
        CancellationToken cancellationToken = default
    );

    Task<TPermission> GetPermissionAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public Task<bool> ExistsPermissionAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<TRolePermission> CreatePermissionToRoleAssociationAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken = default
    );

    Task<TRolePermission> CreatePermissionToRoleAssociationAsync(
        TRole role,
        TPermission permission,
        CancellationToken cancellationToken = default
    );

    Task DeleteRoleToPermissionAssociationAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken = default
        );

    Task DeleteRoleToPermissionAssociationAsync(
        TRole role,
        TPermission permission,
        CancellationToken cancellationToken = default
    );

    Task<TRolePermission> GetRoleToPermissionAssociationAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
}