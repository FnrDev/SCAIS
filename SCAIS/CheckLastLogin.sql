-- Query to check last_login values for all users
SELECT 
    user_id,
    email,
    role,
    created_date,
    last_login,
    is_active,
    CASE 
        WHEN last_login IS NULL THEN 'Never logged in'
        ELSE CONVERT(VARCHAR, last_login, 120)
    END AS last_login_formatted
FROM users
ORDER BY user_id;

-- Query to check recent login activity
SELECT 
    user_id,
    email,
    role,
    last_login,
    DATEDIFF(MINUTE, last_login, GETDATE()) AS minutes_since_last_login
FROM users
WHERE last_login IS NOT NULL
ORDER BY last_login DESC;
