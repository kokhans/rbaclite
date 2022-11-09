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

using System.Collections.Concurrent;
using Carcass.Core;
using RbacLite.InMemory.Models;
using RbacLite.InMemory.Providers.Abstracts;

namespace RbacLite.InMemory.Providers;

public sealed class InMemoryRbacProvider : IInMemoryRbacProvider
{
    private readonly ConcurrentDictionary<Guid, Role> _roles;
    private readonly ConcurrentDictionary<Guid, Permission> _permissions;
    private readonly ConcurrentDictionary<Guid, RolePermission> _rolePermissions;

    public InMemoryRbacProvider()
    {
        _roles = new ConcurrentDictionary<Guid, Role>();
        _permissions = new ConcurrentDictionary<Guid, Permission>();
        _rolePermissions = new ConcurrentDictionary<Guid, RolePermission>();
    }

    public Task<Role> CreateRoleAsync(
        string systemName,
        string displayName,
        string? description,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotNull(systemName, nameof(systemName));
        ArgumentVerifier.NotNull(displayName, nameof(displayName));

        Guid id = Guid.NewGuid();
        Role role = new()
        {
            Id = id,
            SystemName = systemName,
            DisplayName = displayName,
            Description = description
        };
        bool created = _roles.TryAdd(id, role);
        if (!created)
            throw new InvalidOperationException($"Create role {systemName} failed.");

        return Task.FromResult(role);
    }

    public Task<Role> UpdateRoleAsync(
        Role role,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotNull(role, nameof(role));

        bool updated = _roles.TryUpdate(
            role.Id,
            role,
            GetRoleAsync(role.Id, cancellationToken).Result
        );
        if (!updated)
            throw new InvalidOperationException($"Update role {role.Id} failed.");

        return Task.FromResult(role);
    }

    public Task DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotDefault(id, nameof(id));

        bool deleted = _roles.Remove(
            GetRoleAsync(id, cancellationToken).Result.Id,
            out Role? _
        );
        if (!deleted)
            throw new InvalidOperationException($"Delete role {id} failed.");

        return Task.CompletedTask;
    }

    public Task DeleteRoleAsync(
        Role role,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotNull(role, nameof(role));

        return DeleteRoleAsync(role.Id, cancellationToken);
    }

    public Task<Role> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotDefault(id, nameof(id));

        bool founded = _roles.TryGetValue(id, out Role? role);
        if (!founded)
            throw new InvalidOperationException($"Role {id} not found");

        return Task.FromResult(role!);
    }

    public Task<bool> ExistsRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotDefault(id, nameof(id));

        return Task.FromResult(_roles.TryGetValue(id, out Role? _));
    }

    public Task<Permission> CreatePermissionAsync(
        string systemName,
        string displayName,
        string? description = default,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotNull(systemName, nameof(systemName));
        ArgumentVerifier.NotNull(displayName, nameof(displayName));

        Guid id = Guid.NewGuid();
        Permission permission = new()
        {
            Id = id,
            SystemName = systemName,
            DisplayName = displayName,
            Description = description
        };
        bool created = _permissions.TryAdd(id, permission);
        if (!created)
            throw new InvalidOperationException($"Create permission {systemName} failed.");

        return Task.FromResult(permission);
    }

    public Task<Permission> UpdatePermissionAsync(
        Permission permission,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotNull(permission, nameof(permission));

        bool updated = _permissions.TryUpdate(
            permission.Id,
            permission,
            GetPermissionAsync(permission.Id, cancellationToken).Result
        );
        if (!updated)
            throw new InvalidOperationException($"Update permission {permission.Id} failed.");

        return Task.FromResult(permission);
    }

    public Task DeletePermissionAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotDefault(id, nameof(id));

        bool deleted = _permissions.Remove(
            GetPermissionAsync(id, cancellationToken).Result.Id,
            out Permission? _
        );
        if (!deleted)
            throw new InvalidOperationException($"Delete permission {id} failed.");

        return Task.CompletedTask;
    }

    public Task DeletePermissionAsync(
        Permission permission,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotNull(permission, nameof(permission));

        return DeletePermissionAsync(permission.Id, cancellationToken);
    }

    public Task<Permission> GetPermissionAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotDefault(id, nameof(id));

        bool founded = _permissions.TryGetValue(id, out Permission? permission);
        if (!founded)
            throw new InvalidOperationException($"Permission {id} not found.");

        return Task.FromResult(permission!);
    }

    public Task<bool> ExistsPermissionAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotDefault(id, nameof(id));

        return Task.FromResult(_permissions.TryGetValue(id, out Permission? _));
    }

    public Task<RolePermission> CreatePermissionToRoleAssociationAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotDefault(roleId, nameof(roleId));
        ArgumentVerifier.NotDefault(permissionId, nameof(permissionId));

        Guid id = Guid.NewGuid();
        RolePermission rolePermission = new()
        {
            Id = id,
            RoleId = roleId,
            PermissionId = permissionId
        };
        bool created = _rolePermissions.TryAdd(id, rolePermission);
        if (!created)
            throw new InvalidOperationException($"Create permission {permissionId} to role {roleId} association failed.");

        return Task.FromResult(rolePermission);
    }

    public Task<RolePermission> CreatePermissionToRoleAssociationAsync(
        Role role,
        Permission permission,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotNull(role, nameof(role));
        ArgumentVerifier.NotNull(permission, nameof(permission));

        return CreatePermissionToRoleAssociationAsync(
            role.Id,
            permission.Id,
            cancellationToken
        );
    }

    public Task DeleteRoleToPermissionAssociationAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotDefault(roleId, nameof(roleId));
        ArgumentVerifier.NotDefault(permissionId, nameof(permissionId));

        KeyValuePair<Guid, RolePermission> rolePermission =
            _rolePermissions.Single(kvp =>
                kvp.Value.RoleId == roleId && kvp.Value.PermissionId == permissionId
            );

        return Task.CompletedTask;
    }

    public Task DeleteRoleToPermissionAssociationAsync(Role role, Permission permission, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotNull(role, nameof(role));
        ArgumentVerifier.NotNull(permission, nameof(permission));

        return DeleteRoleToPermissionAssociationAsync(role.Id, permission.Id, cancellationToken);
    }

    public Task<RolePermission> GetRoleToPermissionAssociationAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentVerifier.NotDefault(id, nameof(id));

        bool founded = _rolePermissions.TryGetValue(id, out RolePermission? rolePermission);
        if (!founded)
            throw new InvalidOperationException($"Role to permission association {id} not found.");

        return Task.FromResult(rolePermission!);
    }
}