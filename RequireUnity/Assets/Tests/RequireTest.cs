using UnityEngine;
using System.Collections;
using Shouldly;
using Require;

public class RequireTest : TestBehaviour
{
    Transform module;
    Transform sibling;
    Renderer[] renderers;

    public override void Spec()
    {
        Scenario("uses Require");

        Given("it has a parent")
            .And("its parent is a module root")
            .When("it gets the module root")
            .Then("it should return its parent")
            .Because("it should be able to locate its module root when it exists");

        Given("it has a parent")
            .When("it gets the module root")
            .Then("it should return itself")
            .Because("it should consider itself the module if no module root exists");

        Given("it has a parent")
            .And("it is a module root")
            .When("it gets the module root")
            .Then("it should return itself")
            .Because("it should be able to find itself if it is the module root");

        Given("it has a parent")
            .And("its parent is a module root")
            .And("it has a sibling with a renderer")
            .And("its parent has a renderer")
            .When("it gets all renderers in module")
            .Then("it should find 2 renderers")
            .Because("it should be able to find all of a type of component in the module");

        Given("it has a parent")
            .And("its parent is a module root")
            .And("it has a sibling with a renderer")
            .And("its sibling is a module root")
            .And("its parent has a renderer")
            .When("it gets all renderers in module")
            .Then("it should find 1 renderers")
            .Because("it should not treat components in submodules as being in the same module as it");
    }

    public void ItHasAParent()
    {
        GameObject parent = new GameObject("Parent");
        transform.parent = parent.transform;
    }

    public void ItsParentIsAModuleRoot()
    {
        transform.parent.gameObject.AddComponent<ModuleRoot>();
    }

    public void ItIsAModuleRoot()
    {
        gameObject.AddComponent<ModuleRoot>();
    }

    public void ItHasASiblingWithARenderer()
    {
        sibling = new GameObject("Sibling").AddComponent<MeshRenderer>().transform;
        sibling.parent = transform.parent;
    }

    public void ItsSiblingIsAModuleRoot()
    {
        sibling.gameObject.AddComponent<ModuleRoot>();
    }

    public void ItsParentHasARenderer()
    {
        transform.parent.gameObject.AddComponent<MeshRenderer>();
    }

    public void ItGetsAllRenderersInModule()
    {
        module = transform.GetModuleRoot();
        renderers = module.GetComponentsInModule<Renderer>();
    }

    public void ItGetsTheModuleRoot()
    {
        module = transform.GetModuleRoot();
    }

    public void ItShouldReturnItsParent()
    {
        module.ShouldBe(transform.parent);
    }

    public void ItShouldReturnItself()
    {
        module.ShouldBe(transform);
    }

    public void ItShouldFind__Renderers(int num)
    {
        renderers.Length.ShouldBe(num);
    }

}
