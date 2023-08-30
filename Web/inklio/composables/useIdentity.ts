import { JSONResponse, User } from "~~/types/types";
export type Role = "ADMINISTRATOR" | "USER" | "MODERATOR";
export type InklioUser = {
  id: string;
  username: string;
  roles: Role[];
  email_verified: boolean;
  is_active: boolean;
  last_login: Date | null;
  created_at: Date;
};

// Composable to make authentication tasks easier
export default function useIdentity() {
  return {
    inklioUser,
    register,
    login,
    logout,
    isAuthenticated,
    refresh,
    getProfile,
    loginWithGoogle,
    updateProfile,
    deleteAccount,
    resetPassword,
    verifyReset,
    verifyEmail,
    verifyEmailToken,
  };
}

const inklioUser = ref();

/**
 * @desc Register new user
 * @param user User to register
 * @returns {Promise<JSONResponse>}
 */
async function register(user: User): Promise<any> {
  // Attempt register
  return $fetch("/api/iam/authn/register", {
    method: "POST",
    headers: {
      "client-platform": "browser",
    },
    body: user,
  });
}

/**
 * @desc Register new user
 * @param user User to log in
 * @returns {Promise<any>}
 */
async function login(username: string, password: string, isRememberMe: boolean): Promise<any> {
  if (process.server) {
    return inklioUser.value;
  }
  const doLogin = async () => {
      const account = await $fetch(`api/v1/accounts/login`, {
        method: "POST",
        body: { "username": username, "password": password, "is_remember_me": isRememberMe }
      }).catch(a => {
        return inklioUser.value;
      });
      return account;
    };
  const account = await doLogin();
  if (account) {
    inklioUser.value = account;
    localStorage.setItem('inklioUser', JSON.stringify(account));
  }
  return account;
}

async function logout() {
  if (process.server) {
    return;
  }
  const { status } = await $fetch.raw('api/v1/accounts/logout', {
    method: "POST"
  });
  if (status >= 200 && status < 300) {
    inklioUser.value = null;
    localStorage.removeItem('inklioUser');
  }
}

/**
 * @desc Update user profile
 * @returns {Promise<any>}
 */

async function updateProfile(user: User): Promise<any> {
  const response = await $fetch("/api/iam/authn/update", {
    method: "PUT",
    body: user,
  });

  return response;
}

/**
 * @desc Get user profile
 * @returns {Promise<any>}
 */
async function getProfile(): Promise<any> {
  const response = await $fetch("/api/v1/accounts/claims");
  // const response = await $fetch("/auth/test/2");
  return response;
}

/**
 * @desc Receives user token from Google login, and signs user
 * @param token Access token received from Google after login
 * @returns {Promise<any>}
 */
async function loginWithGoogle(token: string): Promise<any> {
  const response = await $fetch("/api/iam/authn/login-google", {
    method: "POST",
    headers: {
      "client-platform": "browser",
    },
    body: {
      token: token,
    },
  });

  return response;
}

/**
 * @desc Returns true/false depending on whether the user is logged in or not
 * @returns {Promise<boolean>}
 */
async function isAuthenticated(): Promise<boolean> {
  // Api response always has status, data, or error
  const authnResult = await $fetch("/api/iam/authn/isauthenticated", {
    headers: {
      "client-platform": "browser",
    },
  });

  // If status is success, then user is authenticated, and return true, otherwise return false
  return authnResult === "success";
}

/**
 * @desc Attempts to refresh tokens
 * @returns {Promise<any>}
 */
async function refresh(): Promise<any> {
  const response = await $fetch("/api/iam/authn/refresh", {
    method: "POST",
    headers: {
      "client-platform": "browser",
    },
  });

  return response;
}

/**
 * @desc Delete user account
 * @returns {Promise<any>}
 */
async function deleteAccount(
  uuid: string,
  csrfToken: string
): Promise<any> {
  const response = await $fetch("/api/iam/authn/delete", {
    method: "DELETE",
    headers: {
      "client-platform": "browser",
    },
    body: {
      uuid: uuid,
      csrf_token: csrfToken,
    },
  });

  return response;
}

/**
 * @desc Reset user's password
 * @returns {Promise<any>}
 */
async function resetPassword(email: string): Promise<any> {
  const response = await $fetch("/api/iam/authn/reset", {
    method: "POST",
    headers: {
      "client-platform": "browser",
    },
    body: {
      email: email,
    },
  });

  return response;
}

/**
 * @desc Verify reset password token sent by user
 * @returns {Promise<any>}
 */
async function verifyReset(token: string): Promise<any> {
  const response = await $fetch("/api/iam/authn/verifyreset", {
    method: "POST",
    headers: {
      "client-platform": "browser",
    },
    body: {
      token: token,
    },
  });

  return response;
}

/**
 * @desc Verify user email after registration
 * @returns {Promise<any>}
 */
async function verifyEmail(email: string): Promise<any> {
  const response = await $fetch("/api/iam/authn/verifyemail", {
    method: "POST",
    headers: {
      "client-platform": "browser",
    },
    body: {
      email: email,
    },
  });

  return response;
}

/**
 * @desc Verify email verification token sent by user
 * @returns {Promise<any>}
 */
async function verifyEmailToken(token: string): Promise<any> {
  const response = await $fetch("/api/iam/authn/verifyemailtoken", {
    method: "POST",
    headers: {
      "client-platform": "browser",
    },
    body: {
      token: token,
    },
  });

  return response;
}
