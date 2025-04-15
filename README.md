# InfoTrack SEO
Creator: Ethan Ford

## Brief
The CEO from InfoTrack is very interested in SEO and how this can improve Sales. Every morning he searches the keyword 
“land registry searches” in Google and counts down to see where and how many times their company, www.infotrack.co.uk 
sits on the list. Seeing the CEO do this every day, a smart software developer at InfoTrack decides to write a small 
application for him that will automatically perform this operation and return the result to the screen. They design and 
code some software that receives a string of keywords, and a string URL. This is then processed to return a string of 
numbers for where the resulting URL is found in Google. For example “1, 10, 33” or “0”. The CEO is only interested if 
their URL appears in the first 100 results.

## Prerequisites
- .Net 8.0 sdk
- Node >=v21.1.0

## Running The Application
1. Restore NuGet packages in API.
2. Run `npm install`
3. Run `npm run start:api`
4. Run `npm run start:app` (in another terminal)

## NB
Google's anti-scraping and bot detection may impact searching. 
I have set up a dummy object that gets returned if this is the case to showcase a successful response.
This can be toggled in the `appsettings.json` file in the API.

## Using The Application
This is broken up into 4 major components. 
1. Search Screen - The screen where you can enter the keywords and search target you would like to show up.
2. Result Screen - The screen where you can view your most recent search.
3. History Screen - The screen which shows the history of what was searched.
4. Trends Screen - The screen which allows you to view various graphs showcasing the results.

## Description
This is a React SPA and .Net Core API. [Shadcn/ui](https://ui.shadcn.com/) and [Tailwind CSS](https://tailwindcss.com/)
has been used for the UI components. [React Query](https://tanstack.com/query/latest) has been used to manage state 
and [ky](https://www.npmjs.com/package/ky) has been used as a lightweight way of making API requests. The build tool
used is [vite](https://vite.dev/). SQLite was also chosen as the database for a lightweight way to store data.


## Future Improvements
- Use of HTTPS. HTTP was chosen for brevity.
- Introducing sorting/filtering to history list.
- Additional filtering of graphs/plots + new plots. This could lead to use of a dashboard.
- Scheduling. I think something like this would benefit greatly if something like Hangfire or Quartz was used to 
schedule these searches. 
- Additional pages, usage of React Router to handle the navigation.
- Additional Search Engines/way of adding new ones dynamically.