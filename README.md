# Development of a Web Tool for Solving and Visualizing the “Contain Virus” Problem in 2D Grids

This project is a full-stack ASP.NET Core application designed to simulate and visualize the **Contain Virus algorithm**, a popular algorithmic challenge involving the containment of contagious regions on a 2D grid. The system generates infection patterns, runs the algorithm step-by-step, and visually demonstrates how containment decisions affect the virus spread over time.

This work was developed as part of a **Bachelor thesis** focusing on algorithmic modeling, grid-based simulations, and visualization of the containment decision-making process.

---

## Live Demo  
🔗 **https://containvirus.onrender.com/**

---

## Algorithmic Concepts

The Contain Virus algorithm models the spread of an infection on a 2D grid where:

- **1** = infected  
- **0** = healthy  
- **-1** = quarantined  

At each simulation “day,” the algorithm performs several steps grounded in classic algorithmic techniques — primarily **Breadth-First Search (BFS)** and the **Greedy strategy**.

### **1. Region Detection (BFS)**
Using **Breadth-First Search**, every connected group of infected cells is identified as a separate region.  
For each region, BFS determines:

- The infected cells belonging to that region  
- The **frontier cells** (healthy neighbors at risk)  
- The number of **walls** required to isolate it  

### **2. Greedy Evaluation of Regions**
The core of the algorithm uses a **Greedy strategy**:

> **At each step, isolate the region that threatens to infect the largest number of cells in the next day.**

This is determined by the size of the region’s **frontier**.  
The region with the largest frontier is considered the most dangerous and is selected for containment.

This decision is *locally optimal* — it tries to minimize tomorrow’s spread rather than computing an expensive global optimum over many days.

### **3. Containing the Most Dangerous Region**
Once the region with the largest frontier is chosen, the algorithm builds walls around it.  
All cells in the isolated region become **-1**, preventing any future spread.

### **4. Natural Spread of Remaining Regions**
All other infected regions spread normally:
their frontier cells become infected (`1`), simulating an uncontrolled infection.

### **5. Termination Condition**
The simulation stops when:

- No region has frontier cells (nothing left to infect)  
- All infected cells are either contained or unable to expand  

This approach models a practical containment strategy under a strict constraint:  
**only one region can be quarantined per day**, making the Greedy choice the most feasible decision-making method.

---

## Running the Project Locally

### 1. Clone the repository
```bash
git clone https://github.com/mirgetagashi/ContainVirus.git
cd ContainVirus
dotnet restore
dotnet run
